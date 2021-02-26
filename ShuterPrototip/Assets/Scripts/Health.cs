using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] int health = 100;
    [SerializeField] int curentHealth;
    public Collider2D hitBox;
    public Image healthBar;

    public event Action Death = default;
    void Start()
    {
        curentHealth = health;
    }
    private float map(float x, float in_min, float in_max, float out_min, float out_max)
    {
        return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
    }
    public void TakDamage (int damage)
    {
        curentHealth = Mathf.Clamp(curentHealth -= damage, 0, health);
        healthBar.fillAmount = map(Mathf.Lerp(health, curentHealth, 1), 0f, health, 0f, 1f);
        if (curentHealth <= 0 && Death != null)
        {
            Death();
            hitBox.enabled = false; 
            healthBar.canvas.enabled = false;
            enabled = false;
        }
    }
}
