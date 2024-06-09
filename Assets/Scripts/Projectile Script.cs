using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tourelle2 : MonoBehaviour
{
    public GameObject ballePrefab;
    public Transform canonTransform;
    public float force = 2f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Tirer();
        }
    }

    void Tirer()
    {
        GameObject balle = Instantiate(ballePrefab, canonTransform.position, canonTransform.transform.rotation);
        Rigidbody rb = balle.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.AddForce(canonTransform.forward * force, ForceMode.Impulse);
        }
    }
}
