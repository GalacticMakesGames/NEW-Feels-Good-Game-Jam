using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 20;
    public int currentHealth;

    [SerializeField] private Animator playerAnimator;

    public GameOverManager gameOverManager;
    public PlayerDashAttack playerDashAttack;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void PlayerTakeDamage(int amount)
    {
        playerAnimator.SetTrigger("isHit");
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            if (playerDashAttack.isDashing == false)
            {
                playerAnimator.SetBool("isIdle", false);
                playerAnimator.SetBool("isDead", true);

                StartCoroutine(PlayerDeathSequence());
            }
        }
    }

    private IEnumerator PlayerDeathSequence()
    {
        yield return new WaitForSeconds(1f);

        Time.timeScale = 0f;

        gameOverManager.GameOver();
        Debug.Log("Game Over!");
    }
}
