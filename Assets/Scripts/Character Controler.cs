using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CharacterControler : MonoBehaviour
{
    [Header("Movement")]
    float moveSpeed;
    public float baseSpeed;
    public float sprintSpeedMultiplier;
    public float slowSpeedMultiplier;
    public bool grounded;
    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump = true;

    CamPos camPos;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("GroundCheck")]
    public float playerHeight;
    public LayerMask ground;

    public Transform orientation;
    public Transform character;

    float horizontalInput;
    float verticalInput;
    Vector3 moveDirection;
    Rigidbody rb;

    public bool sprintKeyPush;
    public bool isSprinting;
    public bool isCrouch;
    States states;

    void Awake()
    {
        moveSpeed = baseSpeed;
        states = GetComponent<States>();
        rb = GetComponent<Rigidbody>();
        camPos = GameObject.Find("CameraHolder").GetComponent<CamPos>();
    }

    void Start()
    {
        /*
        moveSpeed = baseSpeed;
        states = GetComponent<States>();
        rb = GetComponent<Rigidbody>();
        camPos = GameObject.Find("CameraHolder").GetComponent<CamPos>();
        */
    }

    void FixedUpdate()
    {
        MovePlayer();

        /*if(!IsOwner)
        {
            return;
        }
        if(IsServer && IsLocalPlayer)
        {
            MovePlayer();
        }
        else if(IsClient && IsLocalPlayer)
        {
            MovePlayerServerRPC();
            Debug.Log("IsClient");
        }*/
    }
    void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 1f + 0.2f, ground);

        MyInput();
        SpeedControl();
        CharacterOrientation();

        if (grounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }
    }

    void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(sprintKey))
        {
            isSprinting = states.CheckCanSprint(sprintKeyPush = true);
            states.setStaminaController(sprintKeyPush = true);
        }
        else if (!Input.GetKey(sprintKey))
        {
            isSprinting = states.CheckCanSprint(sprintKeyPush = false);
            states.setStaminaController(sprintKeyPush = false);
        }

        switch (Input.GetKey(crouchKey))
        {
            case true:
                isCrouch = true;
                //camPos.Crouch();
                break;
            case false:
                isCrouch = false;
                //camPos.Crouch();
                break;
        }

        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }

        if (horizontalInput == 0 && verticalInput == 0)
        {
            moveSpeed = 1.5f;
        }
        else
        {
            moveSpeed = baseSpeed;
        }
    }
    void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        //Debug.Log(moveDirection);

        if (grounded && !isSprinting && !isCrouch)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        else if (grounded && isSprinting)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * sprintSpeedMultiplier, ForceMode.Force);
        }
        else if (grounded && isCrouch)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * slowSpeedMultiplier, ForceMode.Force);
        }
        else if (!grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
    }

    void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    void ResetJump()
    {
        readyToJump = true;
    }

    void CharacterOrientation()
    {
        character.forward = orientation.forward;
    }
}
