using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public States states;

    void Start()
    {
        states = GameObject.Find("Player").GetComponent<States>();
    }
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("AIE");
        states.setHPController(-25);
        states.setStaminaController(-75);
    }
}
