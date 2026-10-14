using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashAttack : MonoBehaviour
{
    public float dashSpeed = 20f;
    public float stoppingDistance = 0.5f;

    private Vector2 startPosition;
    private bool isDashing = false;

    // Define the movement states
    private enum DashState { Idle, MovingToEnemy, Returning }
    private DashState currentState = DashState.Idle;

    public Enemy enemy;

    public void StartDashAttack()
    {
        if (isDashing) return;

        StartCoroutine(ExecuteDashSequence(enemy.transform));
    }

    private IEnumerator ExecuteDashSequence(Transform enemyTransform)
    {
        isDashing = true;

        // Record player starting position before moving
        startPosition = transform.position;

        // Dash to enemy
        currentState = DashState.MovingToEnemy;
        while (Vector2.Distance(transform.position, enemyTransform.position) > stoppingDistance)
        {
            // Frame-rate independent movement towards the enemy
            transform.position = Vector2.MoveTowards(
                transform.position,
                enemyTransform.position,
                dashSpeed * Time.deltaTime
            );
            yield return null; // Wait for the next frame
        }

        // Optional: Insert logic here to apply damage or play an impact effect
        enemy.TakeDamage();

        // Return to player starting position
        currentState = DashState.Returning;
        while (Vector2.Distance(transform.position, startPosition) > 0.01f)
        {
            // Frame-rate independent movement back to start
            transform.position = Vector2.MoveTowards(
                transform.position,
                startPosition,
                dashSpeed * Time.deltaTime
            );
            yield return null;
        }

        // Sequence complete, reset player state
        transform.position = startPosition; // Snap perfectly to start position to avoid minor offsets
        currentState = DashState.Idle;
        isDashing = false;
    }
}
