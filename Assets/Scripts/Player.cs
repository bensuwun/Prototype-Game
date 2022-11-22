using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;

    public HealthBar healthBar;

    bool isRunning = false;

    // void Start()
    // {
    //     currentHealth = maxHealth;
    //     healthBar.SetMaxHealth(maxHealth);
    // }

    void Update()
    {
        StartCoroutine(healthDecay(currentHealth));

        // to be changed adjusted -> Press spacebar to take damage for now
        if (Input.GetKeyDown(KeyCode.Backspace)) 
        {
            TakeDamage(3);
        } 
    }

    public void setMaxHP(float maxHP) {
        maxHealth = maxHP;
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    public void TakeDamage(float damage)
    {
        if (currentHealth > 0)
            currentHealth -= damage;

        healthBar.SetHealth(currentHealth);
    }

    public void increaseHealth(float increase)
    {
        if(currentHealth != maxHealth)
            currentHealth += increase;
        
        healthBar.SetHealth(currentHealth);
    }

      IEnumerator healthDecay(float currentHealth)
    {
        if(isRunning)
            yield break; // exit if still running
        
        isRunning = true;

        while (true)   
        {
            TakeDamage(0.0007f);  //to be adjusted
            yield return null;
        }
    }

    public bool isPlayerDead() {
        if (currentHealth <= 0) {
            return true;
        }
        return false;
    }
}
