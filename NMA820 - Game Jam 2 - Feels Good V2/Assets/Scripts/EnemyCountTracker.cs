using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCountTracker : MonoBehaviour
{
    public static List<EnemyCountTracker> activeEnemyInstances = new List<EnemyCountTracker>();

    private void OnEnable()
    {
        activeEnemyInstances.Add(this);
    }

    private void OnDisable()
    {
        activeEnemyInstances.Remove(this);
    }
}
