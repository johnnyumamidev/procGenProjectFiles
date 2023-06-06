using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    Enemy enemy;
    public EnemyState currentState;
    public LayerMask playerLayer;
    public LayerMask obstaclesLayer;

    public Vector2 targetPosition;
    public enum EnemyState
    {
        Patrol, Chase, Attack, Search, Dead
    }
    private void Awake()
    {
        enemy = GetComponent<Enemy>();
    }
    private void Start()
    {
        currentState = EnemyState.Patrol;
        SetEnemyState(currentState);   
    }

    public void HandleAI()
    {
        DetectPlayerSphere();
    }

    private void SetEnemyState(EnemyState newState)
    {
        currentState = newState;
    }
    private void DetectPlayerSphere()
    {
        Collider2D overlap = Physics2D.OverlapCircle(transform.position, enemy.enemyData.detectPlayerRadius, playerLayer);
        if (overlap)
        {
            Vector2 playerPosition = overlap.transform.position;
            Vector2 enemyPosition = transform.position;
            float rayCastRange = Vector2.Distance(playerPosition, enemyPosition);
            RaycastHit2D lineOfSight = Physics2D.Raycast(enemyPosition, DirectionToTarget(playerPosition, enemyPosition), rayCastRange, obstaclesLayer);

            if (lineOfSight)
            {
                DirectionToTarget(lineOfSight.point, enemyPosition);
                SetEnemyState(EnemyState.Search);
            }
            else
            {
                DirectionToTarget(playerPosition, enemyPosition);
                SetEnemyState(EnemyState.Chase);
            }
        }
    }

    private Vector2 DirectionToTarget(Vector2 startPosition, Vector2 endPosition)
    {
        Vector2 _directionToTarget = startPosition - endPosition;
        directionToTarget = _directionToTarget;
        return _directionToTarget;
    }

    Vector2 directionToTarget;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, directionToTarget);
        Gizmos.color = Color.yellow;
        if(enemy != null) Gizmos.DrawWireSphere(transform.position, enemy.enemyData.detectPlayerRadius);
    }
}
