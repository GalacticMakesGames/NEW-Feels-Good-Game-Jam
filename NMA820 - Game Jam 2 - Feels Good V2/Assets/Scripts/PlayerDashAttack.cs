using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashAttack : MonoBehaviour
{
    public float dashSpeed = 20f;
    public float stoppingDistance = 0.5f;

    private Vector3 startPosition;
    public bool isDashing = false;

    // Define the movement states
    private enum DashState { Idle, MovingToEnemy, Returning }
    private DashState currentState = DashState.Idle;

    public Enemy enemy;

    [SerializeField] private Animator playerAnimator;

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
            playerAnimator.SetBool("isSprinting", false);
            playerAnimator.SetBool("isAttacking", true);
            targetEnemy.TakeDamage();
        }
        yield return new WaitForSeconds(1f);

        // Return to player starting position
        currentState = DashState.Returning;
        while (Vector3.Distance(transform.position, startPosition) > 0.01f)
        {
            playerAnimator.SetBool("isAttacking", false);
            playerAnimator.SetBool("isSprinting", true);

            // Frame-rate independent movement back to start
            transform.position = Vector3.MoveTowards(
                transform.position,
                startPosition,
                dashSpeed * Time.deltaTime
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
