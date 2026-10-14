using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 20;
    public int currentHealth;
    
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void PlayerTakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Debug.Log("Game Over!");
        }
    }
}
