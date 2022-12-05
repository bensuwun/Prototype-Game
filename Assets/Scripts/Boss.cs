using System;
using UnityEngine;
using TMPro;

public class Boss : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;

    public Healthbar_boss healthBar;
    public TMP_Text healthText;

    public void setMaxHP(float maxHP) {
        maxHealth = maxHP;
        currentHealth = maxHealth;
        healthText.text = String.Format("{0} / {0}", maxHP);
        healthBar.SetMaxHealth(maxHealth);
    }

    public void TakeDamage(float damage, double targetWPM, double wpm, int comboCount)
    {
        if (wpm >= targetWPM) {
            if (currentHealth > 0) {
                if (comboCount > 0) {
                    currentHealth -= (float)(damage * 0.05 * comboCount);
                }   
                else currentHealth -= damage;
                print("Damage was dealt to boss");
                healthText.text = String.Format("{0} / {1}", (int)currentHealth, maxHealth);
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
