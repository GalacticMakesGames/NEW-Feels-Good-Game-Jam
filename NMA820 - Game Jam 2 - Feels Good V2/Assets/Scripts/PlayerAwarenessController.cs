using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAwarenessController : MonoBehaviour
{
    public bool awareOfPlayer { get; private set; }

    public Vector2 directionToPlayer { get; private set; }

    [SerializeField]
    private float playerAwarenessDistance;

    private Transform player;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
    }

    void Update()
    {
        Vector2 enemyToPlayerVector = player.position - transform.position; // says how far away the player is and in what direction
        directionToPlayer = enemyToPlayerVector.normalized;

        if (enemyToPlayerVector.magnitude <= playerAwarenessDistance)
        {
            awareOfPlayer = true;
        }
        else
        {
            awareOfPlayer = false;
        }
    }
}
