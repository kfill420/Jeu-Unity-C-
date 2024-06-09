using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class txtStamina : MonoBehaviour
{
    public States states;
    private Text staminatext;

    void Start()
    {
        states = GameObject.Find("Player").GetComponent<States>();

        staminatext = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        staminatext.text = "stamina" + Mathf.Round(((int)states.currentStatmina));
    }
}
