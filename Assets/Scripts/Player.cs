using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;

    public HealthBar healthBar;
    public TMP_Text healthText;

    bool isRunning = false;

    // void Start()
    // {
    //     currentHealth = maxHealth;
    //     healthBar.SetMaxHealth(maxHealth);
    // }

    void Update()
    {

        // to be changed adjusted -> Press spacebar to take damage for now
        if (Input.GetKeyDown(KeyCode.Backspace)) 
        {
            TakeDamage(3);
        } 
    }

    public void BeginDecay() {
        StartCoroutine(healthDecay(currentHealth));
    }
    

    public void setMaxHP(float maxHP) {
        maxHealth = maxHP;
        currentHealth = maxHealth;
        healthText.text = String.Format("{0} / {0}", maxHP); 
        healthBar.SetMaxHealth(maxHealth);
    }

    public void TakeDamage(float damage)
    {
        if (currentHealth > 0)
            currentHealth -= damage;
        healthText.text = String.Format("{0} / {1}", (int)currentHealth, maxHealth);
        healthBar.SetHealth(currentHealth);
    }

    public void increaseHealth(float increase)
    {
        if(currentHealth != maxHealth)
            currentHealth += increase;
        
        healthText.text = String.Format("{0} / {1}", (int)currentHealth, maxHealth);
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
