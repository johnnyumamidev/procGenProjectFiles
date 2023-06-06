using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyData enemyData;
    EnemyAttack enemyAttack;
    EnemyMovement enemyMovement;
    EnemyHealth enemyHealth;
    EnemyAI enemyAI;

    private void Awake()
    {
        enemyAttack= GetComponent<EnemyAttack>();
        enemyMovement= GetComponent<EnemyMovement>();
        enemyHealth= GetComponent<EnemyHealth>();
        enemyAI= GetComponent<EnemyAI>();
    }
    private void Update()
    {
        enemyAI.HandleAI();
        enemyMovement.HandleAllMovement();
        enemyHealth.HandleHealth();
    }
}
