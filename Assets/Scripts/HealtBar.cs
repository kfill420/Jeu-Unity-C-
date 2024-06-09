using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealtBar : MonoBehaviour
{
    public States states;
    public Slider healthSlider;
    public Slider staminaSlider;
    public TextMeshProUGUI textStamina;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (states != null)
        {
            healthSlider.value = states.currentHealth;
            staminaSlider.value = states.currentStatmina;
            textStamina.text = states.currentStatmina.ToString();
        }
    }
}
