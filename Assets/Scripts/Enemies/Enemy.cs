using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyData enemyData;
    EnemyHealth enemyHealth;
    EnemyStates enemyStates;
    EnemyAI enemyAI;

    private void Awake()
    {
        transform.name = transform.name.Replace("(Clone)", "").Trim();
        enemyStates = GetComponent<EnemyStates>();
        enemyHealth = GetComponent<EnemyHealth>();
        enemyAI = GetComponent<EnemyAI>();
    }
    
    private void Update()
    {
        enemyHealth.HandleHealth();
        enemyStates.HandleStates();
        enemyAI.HandleAllMovement();
    }
}
