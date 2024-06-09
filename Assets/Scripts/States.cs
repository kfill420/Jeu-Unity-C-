using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class States : MonoBehaviour
{
    public int currentHealth;
    private int maxHealth = 100;
    public float currentStatmina;
    private float maxStamina = 100;
    public float staminaIncrementRate = 2;
    public bool coroutineInProgress;
    public bool sprintingBlocked;


    void Start()
    {
        currentHealth = maxHealth;
        currentStatmina = maxStamina;
    }

    void Update()
    {
        // juste pour tester
        if (Input.GetKeyDown(KeyCode.H))
        {
            setHPController(10);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            setHPController(-10);
        }
    }

    public void setHPController(int damage)
    {
        currentHealth += damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Methode pour gérer la stamina
    public void setStaminaController(bool sprintingKeyPush)
    {
        currentStatmina = Mathf.Clamp(currentStatmina, 1, maxStamina);
        if (currentStatmina > 5 && sprintingKeyPush && !sprintingBlocked)
        {
            currentStatmina -= staminaIncrementRate * Time.deltaTime;
        }
        else if (!sprintingKeyPush || sprintingKeyPush && sprintingBlocked)
        {
            currentStatmina += staminaIncrementRate * Time.deltaTime;
        }
        else if (currentStatmina <= 5 && !coroutineInProgress)
        {
            coroutineInProgress = true;
            currentStatmina = 6;
            StartCoroutine(DelayStamina(5f));
        }
    }

    //Coroutine pour attendre que le joueur recupere un peu de stamina quand il tombe a 0
    IEnumerator DelayStamina(float delay)
    {
        Debug.Log("attente!");
        sprintingBlocked = true;
        yield return new WaitForSeconds(delay);
        sprintingBlocked = false;
        coroutineInProgress = false;
        Debug.Log("reprend!");
    }

    //Methode pour ajouter ou retirer un montant de stamina
    public void setStaminaController(float IncrementValue)
    {
        currentStatmina = Mathf.Clamp(currentStatmina, 0, maxStamina);
        currentStatmina += IncrementValue;
        if (currentStatmina <= 5)
        {
            coroutineInProgress = true;
            currentStatmina = 6;
            StartCoroutine(DelayStamina(5f));
        }
    }


    // Verifie si le player peut sprint
    public bool CheckCanSprint(bool sprintingKeyPush)
    {
        if (currentStatmina > 5 && sprintingKeyPush && !sprintingBlocked)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void Die()
    {
        Debug.Log("MORT");
    }
}
