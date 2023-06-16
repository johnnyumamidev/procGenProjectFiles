using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStates : MonoBehaviour
{
    Enemy enemy;
    public State currentState;
    public LayerMask playerLayer;
    public LayerMask obstaclesLayer;

    public Vector2 targetPosition;

    public Transform attackHitbox;

    public bool attackReady = true;
    public enum State
    {
        Patrol, Chase, Attack, Search, Dead
    }
    private void Awake()
    {
        enemy = GetComponent<Enemy>();
    }
    private void Start()
    {
        currentState = State.Patrol;
        SetEnemyState(currentState);
    }

    public void HandleStates()
    {
        if (currentState == State.Dead) return;
        DetectPlayerSphere();
        if (!attackReady) return;
        CheckTargetWithinAttackRange();
    }
    public void SetEnemyState(State newState)
    {
        currentState = newState;
    }

    private void CheckTargetWithinAttackRange()
    {
        Collider2D attackRange = Physics2D.OverlapCircle(attackHitbox.position, enemy.enemyData.attackRange, playerLayer);
        if (attackRange)
        {
            SetEnemyState(State.Attack);
        }
    }

    private void DetectPlayerSphere()
    {
        Collider2D overlap = Physics2D.OverlapCircle(transform.position, enemy.enemyData.detectPlayerRadius, playerLayer);
        if (overlap)
        {
            Vector2 playerPosition = overlap.transform.position;
            targetPosition = playerPosition;
            Vector2 enemyPosition = transform.position;
            float rayCastRange = Vector2.Distance(playerPosition, enemyPosition);
            RaycastHit2D lineOfSight = Physics2D.Raycast(enemyPosition, DirectionToTarget(playerPosition, enemyPosition), rayCastRange, obstaclesLayer);

            if (currentState == State.Attack) return;
            else
            {
                DirectionToTarget(playerPosition, enemyPosition);
                SetEnemyState(State.Chase);
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
        if (enemy != null)
        {
            Gizmos.DrawWireSphere(transform.position, enemy.enemyData.detectPlayerRadius);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackHitbox.position, enemy.enemyData.attackRange);
        } 
    }
}
