using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    Enemy enemy;
    EnemyAI enemyAi;
    Rigidbody2D enemyRigidbody;

    Transform westWaypoint;
    Transform eastWaypoint;
    [SerializeField] Transform lastWaypointReached;
    public float waypointOffsetMultiplier = 0.25f;
    Vector2 velocity;
    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        enemyAi = GetComponent<EnemyAI>();
        enemyRigidbody = GetComponent<Rigidbody2D>();
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

        westWaypoint.position = enemyPosition + (Vector2.left * waypointOffsetMultiplier);
        eastWaypoint.position = enemyPosition + (Vector2.right * waypointOffsetMultiplier);
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
        if (Vector2.Distance(transform.position, westWaypoint.position) < 0.1f) lastWaypointReached = westWaypoint;
        else if (Vector2.Distance(transform.position, eastWaypoint.position) < 0.1f) lastWaypointReached = eastWaypoint;

        Vector2 currentPosition = transform.position;
        if (lastWaypointReached == null) velocity = Vector2.right * enemy.enemyData.speed * Time.fixedDeltaTime;

        if (lastWaypointReached == eastWaypoint) velocity = Vector2.left * enemy.enemyData.speed * Time.fixedDeltaTime;
        else if (lastWaypointReached == westWaypoint) velocity = Vector2.right * enemy.enemyData.speed * Time.fixedDeltaTime;

        enemyRigidbody.velocity = velocity;
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
