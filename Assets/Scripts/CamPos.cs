using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamPos : MonoBehaviour
{
    public Transform cameraPosition;
    public CharacterControler CharacterControler;
    bool crouch;
    Vector3 upPosition;
    Vector3 crouchPosition;

    void Update()
    {
        transform.position = cameraPosition.position;
    }

    public void Crouch()
    {
        if (!CharacterControler.isCrouch && crouch)
        {
            cameraPosition.position = new Vector3(cameraPosition.position.x, cameraPosition.position.y + 0.2f, cameraPosition.position.z);
            crouch = false;
        }
        else if (CharacterControler.isCrouch && !crouch)
        {
            cameraPosition.position = new Vector3(cameraPosition.position.x, cameraPosition.position.y - 0.2f, cameraPosition.position.z);
            crouch = true;
        }
    }
}
