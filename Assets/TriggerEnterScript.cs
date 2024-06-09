using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEnterScript : MonoBehaviour
{

    public bool trigger = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            trigger = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            trigger = false;
        };
    }
}
