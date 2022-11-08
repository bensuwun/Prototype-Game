using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public float maxHealth = 100;
    public float currentHealth;

    public Healthbar_boss healthBar;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        if (currentHealth != 0)
            currentHealth -= damage;

        healthBar.SetHealth(currentHealth);
    }
}
