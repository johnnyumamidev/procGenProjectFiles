using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAI : EnemyAI
{
    [Header("Skeleton Variables")]
    public float patrolMultiplier;
    private void Start()
    {
        SpawnPatrolPoints(patrolMultiplier);   
    }
}
