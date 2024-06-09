using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Cinemachine;

public class ServerMovement : NetworkBehaviour
{   
    [Header("Movement")]
    float moveSpeed;
    public float baseSpeed;
    public float sprintSpeed;
    public float crouchSpeed;
    public bool grounded;
    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airSpeed;
    bool readyToJump = true;

    public Transform orientation;
    MyPlayerInput playerInput;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("GroundCheck")]
    public float playerHeight;
    public LayerMask ground;

    Transform playerTransform;
    Rigidbody rb;

    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private AudioListener audioListener;
    public bool sprintKeyPush;
    public bool isSprinting;
    public bool isCrouch;
    States states;

    void Awake()
    {
        playerInput = new();
        playerInput.Enable();

        moveSpeed = baseSpeed;
        states = GetComponent<States>();
        rb = GetComponent<Rigidbody>();
        playerTransform = GetComponent<Transform>();
    }

    void Update()
    {
        if (!IsOwner)
        {
            virtualCamera.Priority = 0;
            return;
        }
        else if (IsOwner)
        {
            audioListener.enabled = true;
            virtualCamera.Priority = 1;
        }

        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 1f + 0.2f, ground);

        Vector2 moveInput = playerInput.Player.Movement.ReadValue<Vector2>();

        MovePlayer(moveInput);

        PlayerOrientation();

        MyInput();

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
    }
    void MovePlayer(Vector2 _input)
    {
        Vector3 calcMove = _input.x * playerTransform.right + _input.y * playerTransform.forward;

        if (calcMove == new Vector3(0, 0, 0))
        {
            moveSpeed = 1.5f;
        }
        else
        {
            moveSpeed = baseSpeed;
        }

        if (grounded && !isSprinting && !isCrouch)
        {
            rb.AddForce(calcMove.normalized * moveSpeed, ForceMode.Force);
        }
        else if (grounded && isSprinting)
        {
            rb.AddForce(calcMove.normalized * sprintSpeed, ForceMode.Force);
        }
        else if (grounded && isCrouch)
        {
            rb.AddForce(calcMove.normalized * crouchSpeed, ForceMode.Force);
        }
        else if (!grounded)
        {
            rb.AddForce(calcMove.normalized * airSpeed, ForceMode.Force);
        }

        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    void PlayerOrientation()
    {
        playerTransform.rotation = orientation.rotation;
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
}