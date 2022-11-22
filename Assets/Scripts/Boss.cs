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

    public void TakeDamage(float damage, int level, double wpm)
    {
        double targetWPM = 0d;

        switch(level) {
            case 1:
                targetWPM = 10d;
                break;
            case 2:
                targetWPM = 20d;
                break;
            case 3:
                targetWPM = 30d;
                break;
            default:
                targetWPM = 0d;
                break;
        }

        if (wpm >= targetWPM) {
            if (currentHealth != 0) {
                currentHealth -= damage;
                print("Damage was dealt to boss");
                healthBar.SetHealth(currentHealth);
            }
            
        }
        else {
            print("WPM not enough!");
        }
    }
}
