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

    public void TakeDamage(float damage)
    {
        if (currentHealth != 0)
            currentHealth -= damage;

        healthBar.SetHealth(currentHealth);
    }
}
