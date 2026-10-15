using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKeyManager : MonoBehaviour
{
    // Singleton pattern so enemies can access this keybind manager
    public static EnemyKeyManager Instance {  get; private set; }

    [Header("Key Configuration")]
    [SerializeField]
    private List<KeyCode> keyPool = new List<KeyCode>()
    {
        KeyCode.A, KeyCode.B, KeyCode.C, KeyCode.D, KeyCode.E, KeyCode.F, KeyCode.G, KeyCode.H, KeyCode.I, KeyCode.J, KeyCode.K, KeyCode.L, KeyCode.M, KeyCode.N, KeyCode.O, KeyCode.P, KeyCode.Q, KeyCode.R, KeyCode.S, KeyCode.T, KeyCode.U, KeyCode.V, KeyCode.W, KeyCode.X, KeyCode.Y, KeyCode.Z
    };

    // Tracking pools
    private List<KeyCode> availableKeys = new List<KeyCode>();
    private List<Enemy> activeEnemies = new List<Enemy>();

    public AttackZoneCollision attackZoneCollision;
    public PlayerDashAttack playerDashAttack;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // Initialize key pool
        availableKeys.AddRange(keyPool);
    }

    private void Update()
    {
        // Check if any key is pressed
        if (Input.anyKeyDown)
        {
            // Loop backwards through active enemies 
            for (int i = activeEnemies.Count - 1; i >= 0; i--)
            {
                Enemy enemy = activeEnemies[i];

                if (enemy == null)
                {
                    activeEnemies.RemoveAt(i);
                    continue;
                }

                // If the player presses the key assigned to this enemy while the enemy is in the attack zone, attack it
                if (enemy.assignedKey != KeyCode.None && Input.GetKeyDown(enemy.assignedKey) && attackZoneCollision.canAttack == true)
                {
                    //enemy.TakeDamage();
                    playerDashAttack.StartDashAttack(enemy);
                    break;
                }
            }
        }
    }

    public void RegisterAndAssignKey(Enemy newEnemy)
    {
        if (availableKeys.Count  == 0)
        {
            Debug.Log("No more keys available in the pool.");
            // Should the spawner pause spawning enemies until there's a key available to prevent enemies from having no key assigned?
            return;
        }

        // Pick a random index from the remaining unassigned keys
        int randomIndex = Random.Range(0, availableKeys.Count);
        KeyCode chosenKey = availableKeys[randomIndex];

        // Assign the key to the enemy
        newEnemy.SetAssignedKey(chosenKey);

        // Remove the key from the pool so no other enemy gets it assigned
        availableKeys.RemoveAt(randomIndex);

        // Track this enemy in the list
        activeEnemies.Add(newEnemy);
    }

    // Frees up the enemy's key back into the pool and removes them from tracking list
    public void ReleaseKeyAndRemoveEnemy(Enemy enemy)
    {
        if (activeEnemies.Contains(enemy))
        {
            // Put the key back into the pool
            if (enemy.assignedKey != KeyCode.None)
            {
                availableKeys.Add(enemy.assignedKey);
            }

            activeEnemies.Remove(enemy);
        }
    }
}
