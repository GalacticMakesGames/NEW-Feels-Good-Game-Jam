using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashAttack : MonoBehaviour
{
    public float dashSpeed = 30f;
    public float stoppingDistance = 0.5f;

    private Vector3 startPosition;
    public bool isDashing { get; private set; } = false;

    // Define the movement states
    private enum DashState { Idle, MovingToEnemy, Returning }
    private DashState currentState = DashState.Idle;

    public Enemy enemy;

    [SerializeField] private Animator playerAnimator;

    private SpriteRenderer spriteRenderer;

    AudioSource audioSource;
    [SerializeField] private AudioClip playerAttackSFX;
    [SerializeField] private AudioClip playerDashSFX;

    public UltimateAttackManager ultimateAttackManager;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    public void StartDashAttack(Enemy targetEnemy)
    {
        if (targetEnemy == null) return;

        if (!isDashing)
        {
            StartCoroutine(ExecuteDashSequence(targetEnemy));
        }
    }

    private IEnumerator ExecuteDashSequence(Enemy targetEnemy)
    {
        isDashing = true;

        // Record player starting position before moving
        startPosition = transform.position;

        Transform enemyTransform = targetEnemy.transform;

        // Dash to enemy
        currentState = DashState.MovingToEnemy;
        while (enemyTransform != null && Vector3.Distance(transform.position, enemyTransform.position) > stoppingDistance)
        {
            playerAnimator.SetBool("isIdle", false);
            playerAnimator.SetBool("isSprinting", true);

            // Frame-rate independent movement towards the enemy
            transform.position = Vector3.MoveTowards(
                transform.position,
                enemyTransform.position,
                dashSpeed * Time.deltaTime
            );
            yield return null; // Wait for the next frame
        }

        // Apply damage to enemy
        if (targetEnemy != null)
        {
            //playerAnimator.SetBool("isSprinting", false);
            //playerAnimator.SetBool("isAttacking", true);
            if (enemyTransform != null)
            {
                // If the enemy is to the left of the player, flip the sprite; otherwise, face right.
                spriteRenderer.flipX = enemyTransform.position.x < transform.position.x;
            }
            playerAnimator.SetTrigger("isAttacking1");

            targetEnemy.TakeDamage();
            ultimateAttackManager.UltimateBarFill(3);

            yield return new WaitForSeconds(0.1f);

            audioSource.pitch = UnityEngine.Random.Range(0.8f, 1.3f);
            audioSource.PlayOneShot(playerAttackSFX);
        }
        yield return new WaitForSeconds(1f);

        // Return to player starting position
        currentState = DashState.Returning;
        while (Vector3.Distance(transform.position, startPosition) > 0.01f)
        {
            //playerAnimator.SetBool("isAttacking", false);
            //playerAnimator.SetBool("isSprinting", true);

            audioSource.pitch = UnityEngine.Random.Range(0.7f, 1.2f);
            audioSource.PlayOneShot(playerDashSFX, 0.5f);

            // Frame-rate independent movement back to start
            transform.position = Vector3.MoveTowards(
                transform.position,
                startPosition,
                dashSpeed + 20f * Time.deltaTime
            );
            yield return null;
        }

        // Sequence complete, reset player state
        transform.position = startPosition; // Snap perfectly to start position to avoid minor offsets
        currentState = DashState.Idle;
        playerAnimator.SetBool("isSprinting", false);
        playerAnimator.SetBool("isIdle", true);
        isDashing = false;
    }
}
