using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float maxHealth = 100;
    public float currentHealth;

    public HealthBar healthBar;

    bool isRunning = false;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    void Update()
    {
        StartCoroutine(healthDecay(currentHealth));

        // to be changed adjusted -> Press spacebar to take damage for now
        if (Input.GetKeyDown(KeyCode.Backspace)) 
        {
            TakeDamage(10);
        }

        if (CustomTyper.isIdle) TakeDamage(10);
 
    }

    public void TakeDamage(float damage)
    {
        if (currentHealth != 0)
            currentHealth -= damage;

        healthBar.SetHealth(currentHealth);
    }

      IEnumerator healthDecay(float currentHealth)
    {
        if(isRunning)
            yield break; // exit if still running
        
        isRunning = true;

        while (true)   
        {
            TakeDamage(0.007f);  //to be adjusted
            yield return null;
        }
    }
}
