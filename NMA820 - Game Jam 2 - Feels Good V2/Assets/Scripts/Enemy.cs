using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] 
    private float moveSpeed;

    //[SerializeField]
    //private float rotationSpeed;

    private Rigidbody2D rb;
    private PlayerAwarenessController playerAwarenessController;
    private Vector2 targetDirection;

    // Holds the unique key this specific enemy needs to be attacked
    public KeyCode assignedKey { get; private set; } = KeyCode.None;

    [SerializeField] private TextMeshProUGUI enemyAttackKeybindPromptText;
    [SerializeField] private Image enemyAttackKeybindPrompt;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerAwarenessController = GetComponent<PlayerAwarenessController>();

        enemyAttackKeybindPrompt.enabled = false;
    }

    private void FixedUpdate()
    {
        UpdateTargetDirection();
        //RotateTowardsTarget();
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

    //private void RotateTowardsTarget()
    //{
    //    if (targetDirection == Vector2.zero)
    //    {
    //        return;
    //    }

    //    Quaternion targetRotation = Quaternion.LookRotation(transform.forward, targetDirection);
    //    Quaternion rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

    //    rb.SetRotation(rotation);
    //}

    private void SetVelocity()
    {
        if (targetDirection == Vector2.zero)
        {
            rb.velocity = Vector2.zero;
        }
        else
        {
            rb.velocity = targetDirection * moveSpeed;
        }
    }

    //private void Awake()
    //{
    //    rb = GetComponent<Rigidbody2D>();
    //    target = GameObject.Find("Player").transform;
    //}

    //private void Update()
    //{
    //    Vector3 direction = (target.position - transform.position).normalized;
    //    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    //    rb.rotation = angle;
    //    moveDirection = direction;
    //}

    //private void FixedUpdate()
    //{
    //    rb.velocity = new Vector2(moveDirection.x, moveDirection.y) * moveSpeed;
    //}

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
        // Tells the manager that this enemy was killed so its key can be returned to the pool
        EnemyKeyManager.Instance.ReleaseKeyAndRemoveEnemy(this);
        Destroy(gameObject, 1f);

        //GameObject enemyClone = Instantiate(enemyPrefab, transform.position, transform.rotation);

        //// Tells the manager that this enemy was killed so its key can be returned to the pool
        //EnemyKeyManager.Instance.ReleaseKeyAndRemoveEnemy(this);

        ////Destroy(gameObject);
        //Destroy(enemyClone);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            var healthComponent = collision.GetComponent<PlayerHealth>();
            var playerDashAttack = collision.GetComponent<PlayerDashAttack>();

            if(healthComponent != null && playerDashAttack.isDashing == false)
            {
                healthComponent.PlayerTakeDamage(1);
            }
        }

        if (collision.tag == "MainAttackZone")
        {
            enemyAttackKeybindPrompt.enabled = true;
        }
    }
}
