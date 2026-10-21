using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Cinemachine;

public class Enemy : MonoBehaviour
{
    [SerializeField] 
    private float moveSpeed;

    private Rigidbody2D rb;
    private PlayerAwarenessController playerAwarenessController;
    private Vector2 targetDirection;

    // Holds the unique key this specific enemy needs to be attacked
    public KeyCode assignedKey { get; private set; } = KeyCode.None;

    [SerializeField] private TextMeshProUGUI enemyAttackKeybindPromptText;
    [SerializeField] private Image enemyAttackKeybindPrompt;

    [SerializeField] private Animator enemyAnimator;

    public GameObject lightBeamPrefab; // Assign your LightBeam prefab here in Inspector
    public float spawnHeightOffset = 10f; // How high "the sky" is above the enemy

    public GameObject enemyExplosionParticlePrefab;

    [SerializeField] private float shakeForce = 1f;
    private CinemachineImpulseSource impulseSource;

    public bool isMovementPaused { get; private set; } = false;

    AudioSource audioSource;
    [SerializeField] private AudioClip enemyAttackSFX;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerAwarenessController = GetComponent<PlayerAwarenessController>();

        impulseSource = GetComponent<CinemachineImpulseSource>();

        audioSource = GetComponent<AudioSource>();

        enemyAttackKeybindPrompt.enabled = false;
    }

    private void FixedUpdate()
    {
        UpdateTargetDirection();
        SetVelocity();
    }

    private void UpdateTargetDirection()
    {
        if (playerAwarenessController.awareOfPlayer)
        {
            targetDirection = playerAwarenessController.directionToPlayer;
        }
        else
        {
            targetDirection = Vector2.zero;
        }
    }

    private void SetVelocity()
    {
        if (isMovementPaused || targetDirection == Vector2.zero)
        {
            rb.velocity = Vector2.zero;
        }
        else
        {
            rb.velocity = targetDirection * moveSpeed;
        }
    }

    // pause enemy movement
    public void PauseMovement()
    {
        isMovementPaused = true;
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
        }
    }

    // unpause enemy movement
    public void ResumeMovement()
    {
        isMovementPaused = false;
    }

    public void SetAssignedKey(KeyCode key)
    {
        assignedKey = key;
        Debug.Log($"{gameObject.name} assigned key: {key}");

        if (enemyAttackKeybindPromptText != null)
        {
            enemyAttackKeybindPromptText.text = assignedKey.ToString(); // Updates UI to show corresponding Keybind assigned
        }
    }

    public void TakeDamage()
    {
        ScoreManager.instance.AddPoint();

        // Disable collider & stop movement immediately so it can't push the player
        BoxCollider2D col = GetComponent<BoxCollider2D>();
        if (col != null) col.enabled = false;

        //rb.isKinematic = true;

        if (rb != null) rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;

        enemyAnimator.SetBool("isWalking", false);
        enemyAnimator.SetBool("isDead", true);

        StartCoroutine(DeathSequence(targetDirection));
    }

    private IEnumerator DeathSequence(Vector2 direction)
    {
        // Wait for the player's animation to finish
        yield return new WaitForSeconds(1f);

        // Calculate the spawn position directly above the enemy
        Vector3 spawnPosition = new Vector3(
            transform.position.x,
            transform.position.y + spawnHeightOffset,
            transform.position.z
        );

        impulseSource.GenerateImpulseWithVelocity(-direction * shakeForce);

        // Tells the manager that this enemy was killed so its key can be returned to the pool
        EnemyKeyManager.Instance.ReleaseKeyAndRemoveEnemy(this);
        Destroy(gameObject);

        // Instantiate the light beam from the sky
        if (lightBeamPrefab != null)
        {
            Instantiate(lightBeamPrefab, spawnPosition, Quaternion.identity);
        }

        // Spawn the particle effect at the hit position
        if (enemyExplosionParticlePrefab != null)
        {
            Instantiate(enemyExplosionParticlePrefab, transform.position, Quaternion.identity);
        }

        yield return new WaitForSeconds(1f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerHealth healthComponent = collision.GetComponentInParent<PlayerHealth>();
        PlayerDashAttack playerDashAttack = collision.GetComponentInParent<PlayerDashAttack>();

        // Enables visibility of keybind prompt when enemies enter the main attack zone
        if (collision.CompareTag("MainAttackZone"))
        {
            if (enemyAttackKeybindPrompt != null)
            {
                enemyAttackKeybindPrompt.enabled = true;
            }
        }

        // Handle player damage (ignore the main attack zone trigger so damage only applies on body contact)
        if (healthComponent != null && !collision.CompareTag("MainAttackZone"))
        {
            bool isDashing = playerDashAttack != null && playerDashAttack.isDashing;

            if (!isDashing)
            {
                Debug.Log("Player was hit!");

                audioSource.pitch = UnityEngine.Random.Range(0.7f, 1.2f);
                audioSource.PlayOneShot(enemyAttackSFX, 0.3f);

                enemyAnimator.SetTrigger("isAttacking");

                healthComponent.PlayerTakeDamage(1);
            }
        }
    }

    // Freeze and eliminate all active enemies currently in the scene
    public static void TriggerUltimateAttackWipe()
    {
        GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemyObj in enemyObjects)
        {
            Enemy enemy = enemyObj.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.PauseMovement();
                enemy.UltimateAttackTakeDamage(); // Triggers death sequence, particle effects and key release
            }
        }
    }

    public void UltimateAttackTakeDamage()
    {
        ScoreManager.instance.AddPoint();

        // Disable collider & stop movement immediately so it can't push the player
        BoxCollider2D col = GetComponent<BoxCollider2D>();
        if (col != null) col.enabled = false;

        //rb.isKinematic = true;

        if (rb != null) rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;

        enemyAnimator.SetBool("isWalking", false);
        enemyAnimator.SetBool("isDead", true);

        StartCoroutine(UltimateAttackDeathSequence(targetDirection));
    }

    private IEnumerator UltimateAttackDeathSequence(Vector2 direction)
    {
        // Wait for the player's animation to finish
        yield return new WaitForSeconds(1f);

        // Calculate the spawn position directly above the enemy
        Vector3 spawnPosition = new Vector3(
            transform.position.x,
            transform.position.y + spawnHeightOffset,
            transform.position.z
        );

        impulseSource.GenerateImpulseWithVelocity(-direction * shakeForce);

        // Tells the manager that this enemy was killed so its key can be returned to the pool
        EnemyKeyManager.Instance.ReleaseKeyAndRemoveEnemy(this);
        Destroy(gameObject);

        // Instantiate the light beam from the sky
        if (lightBeamPrefab != null)
        {
            Instantiate(lightBeamPrefab, spawnPosition, Quaternion.identity);
        }

        // Spawn the particle effect at the hit position
        if (enemyExplosionParticlePrefab != null)
        {
            Instantiate(enemyExplosionParticlePrefab, transform.position, Quaternion.identity);
        }

        if (isMovementPaused)
        {
            ResumeMovement();
        }

        yield return new WaitForSeconds(1f);
    }
}
