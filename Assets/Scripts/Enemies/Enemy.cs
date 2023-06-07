using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyData enemyData;
    EnemyMovement enemyMovement;
    EnemyHealth enemyHealth;
    EnemyAI enemyAI;

    private void Awake()
    {
        enemyMovement= GetComponent<EnemyMovement>();
        enemyHealth= GetComponent<EnemyHealth>();
        enemyAI= GetComponent<EnemyAI>();
    }
    private void Update()
    {
        enemyAI.HandleAI();
        enemyHealth.HandleHealth();
    }
    private void FixedUpdate()
    {
        enemyMovement.HandleAllMovement();
    }
}
