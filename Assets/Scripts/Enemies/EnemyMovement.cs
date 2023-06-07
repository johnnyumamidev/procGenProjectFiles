using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyMovement : MonoBehaviour
{
    Enemy enemy;
    EnemyAI enemyAi;
    Rigidbody2D enemyRigidbody;
    CapsuleCollider2D enemyCollider;
    public CircleCollider2D wallCheckCollider;

    Transform westWaypoint;
    Transform eastWaypoint;
    public Vector2 westwayPointPosition;
    public Vector2 eastwayPointPosition;
    [SerializeField] Transform lastWaypointReached;
    public float waypointOffsetMultiplier = 0.25f;
    Vector2 velocity;

    Vector2 chaseDirection;
    
    bool facingRight = true;
    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        enemyAi = GetComponent<EnemyAI>();
        enemyRigidbody = GetComponent<Rigidbody2D>();
        enemyCollider = GetComponent<CapsuleCollider2D>();
    }

    private void Start()
    {
        SpawnPatrolPoints();
    }

    private void SpawnPatrolPoints()
    {
        Vector2 enemyPosition = transform.position;
        GameObject wayPointParent = new GameObject("waypoint parent");
        GameObject westWayPointObject = new GameObject("west waypoint");
        GameObject eastWayPointObject = new GameObject("east waypoint");
        westWaypoint = westWayPointObject.transform;
        eastWaypoint = eastWayPointObject.transform;
        westWaypoint.parent = wayPointParent.transform;
        eastWaypoint.parent = wayPointParent.transform;

        westWaypoint.position = enemyPosition + (westwayPointPosition * waypointOffsetMultiplier);
        eastWaypoint.position = enemyPosition + (eastwayPointPosition * waypointOffsetMultiplier);
    }

    public void HandleAllMovement()
    {
        if (velocity.x < 0 && facingRight) Flip();
        else if (velocity.x > 0 && !facingRight) Flip();

        if (enemyAi.currentState == EnemyAI.EnemyState.Patrol) PatrolArea();
        if (enemyAi.currentState == EnemyAI.EnemyState.Search) { return; }
        if (enemyAi.currentState == EnemyAI.EnemyState.Chase) ChaseTarget();
        if (enemyAi.currentState == EnemyAI.EnemyState.Attack) LungeAtTarget();

        if (enemyAi.currentState == EnemyAI.EnemyState.Dead)
        {
            enemyRigidbody.isKinematic = true;
            enemyCollider.enabled = false;
        }

       

        enemyRigidbody.velocity = velocity;
    }

    bool isLunging;
    public void SetLungeTrue()
    {
        isLunging = true;
    }
    private void LungeAtTarget()
    {
        velocity = Vector2.zero;
        if (isLunging)
        {
            Vector2 direction = Vector2.right;
            if (!facingRight) direction = Vector2.left;
            velocity = direction * enemy.enemyData.lungeForce;
            isLunging = false;
        }
    }
    private void Flip()
    {
        facingRight = !facingRight;
        float xScale = transform.localScale.x;
        xScale *= -1;
        transform.localScale = new Vector3(xScale, 1, 1);
    }

    private void PatrolArea()
    {
        Debug.Log(gameObject.name + " patrolling");
        if (wallCheckCollider.IsTouchingLayers(enemyAi.obstaclesLayer))
        {
            if (enemyRigidbody.velocity.x > 0) lastWaypointReached = eastWaypoint;
            else { lastWaypointReached = westWaypoint; }
        }
        if (Vector2.Distance(transform.position, westWaypoint.position) < 0.1f) lastWaypointReached = westWaypoint;
        else if (Vector2.Distance(transform.position, eastWaypoint.position) < 0.1f) lastWaypointReached = eastWaypoint;


        Vector2 currentPosition = transform.position;
        if (lastWaypointReached == null) velocity = Vector2.right * enemy.enemyData.speed * Time.fixedDeltaTime;

        if (lastWaypointReached == eastWaypoint) velocity = Vector2.left * enemy.enemyData.speed * Time.fixedDeltaTime;
        else if (lastWaypointReached == westWaypoint) velocity = Vector2.right * enemy.enemyData.speed * Time.fixedDeltaTime;
    }

    private void ChaseTarget()
    {
        chaseDirection = enemyAi.targetPosition - enemyRigidbody.position;
        if (chaseDirection.x > transform.position.x && !facingRight) Flip();
        else if(chaseDirection.x < transform.position.x && facingRight) Flip();
        velocity = chaseDirection.normalized * enemy.enemyData.speed * Time.fixedDeltaTime;
    }
}
