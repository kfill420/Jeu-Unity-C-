using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTurret : MonoBehaviour
{
    public Vector3 direction;
    public RaycastHit hit;
    public float maxDistance;
    public LayerMask layerMask;

    void Update()
    {
        direction =- transform.right;

        Debug.DrawLine(transform.position, transform.position + direction * maxDistance, Color.green);

        if (Physics.Raycast(transform.position, transform.position + direction, out hit, maxDistance, layerMask))
        {
            
        }
        else
        {
            
        }
    }
}
