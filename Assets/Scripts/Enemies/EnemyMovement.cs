using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    Enemy enemy;
    EnemyAI enemyAi;
    public List<GameObject> patrolPoints = new List<GameObject>();
    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        enemyAi= GetComponent<EnemyAI>();
    }

    public void HandleAllMovement()
    {
        if (enemyAi.currentState == EnemyAI.EnemyState.Patrol) PatrolArea();
        if (enemyAi.currentState == EnemyAI.EnemyState.Search) SearchForTarget();
        if (enemyAi.currentState == EnemyAI.EnemyState.Chase) ChaseTarget();
    }

    private void PatrolArea()
    {
        Debug.Log(gameObject.name + " patrolling");
        if(enemy.enemyData.enemyType == "Grounded")
        {

        }
        if (enemy.enemyData.enemyType == "Flying")
        {

        }
    }

    private void ChaseTarget()
    {
        Debug.Log(gameObject.name + " chasing target!");
    }
    private void SearchForTarget()
    {
        Debug.Log(gameObject.name + " searching for target");
    }
}
