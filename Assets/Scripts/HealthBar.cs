
using System;
using UnityEngine;
using UnityEngine.UI;

/**
    HealthBar class to store health bar properties for calculation:
    @param slider (Slider) - health value that corresponds with healh value
    @param gradient (Gradient) - color gradient used for health values
    @param fill (Image) - image for current health
*/
public class HealthBar : MonoBehaviour
{
    public Slider slider; 
    public Gradient gradient;
    public Image fill;

    public void SetMaxHealth(float health)
    {
        slider.maxValue = health;
        slider.value = health;

        fill.color = gradient.Evaluate(1f);
    }

    public void SetHealth(float health)
    {
        slider.value = health;

        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

}
