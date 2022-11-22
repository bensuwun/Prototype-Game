using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;

    public Healthbar_boss healthBar;

    public void setMaxHP(float maxHP) {
        maxHealth = maxHP;
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    public void TakeDamage(float damage, double targetWPM, double wpm)
    {
        if (wpm >= targetWPM) {
            if (currentHealth > 0) {
                currentHealth -= damage;
                print("Damage was dealt to boss");
                healthBar.SetHealth(currentHealth);
            }
            
        }
        else {
            print("WPM not enough!");
        }

        
    }

    public bool isBossDead() {
        if (currentHealth <= 0) {
            return true;
        }
        return false;
    }
}
