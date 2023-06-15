using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static EnemyStates;

public class EnemyAI : MonoBehaviour, IEventListener
{
    Enemy enemy;
    EnemyStates enemyStates;
    Rigidbody2D enemyRigidbody;
    CapsuleCollider2D enemyCollider;
    public CircleCollider2D wallCheckCollider;

    Transform westWaypoint;
    Transform eastWaypoint;
    public Vector2 westWaypointPosition;
    public Vector2 eastWaypointPosition;
    [SerializeField] Transform lastWaypointReached;
    [SerializeField] protected float waypointOffsetMultiplier = 0.25f;
    public Vector2 velocity;

    Vector2 chaseDirection;
    
    bool facingRight = false;
    private void Awake()
    {
        enemyStates = GetComponent<EnemyStates>();
        enemy = GetComponent<Enemy>();
        enemyRigidbody = GetComponent<Rigidbody2D>();
        enemyCollider = GetComponent<CapsuleCollider2D>();
    }
    protected void SpawnPatrolPoints(float _waypointOffsetMultiplier)
    {
        Vector2 enemyPosition = transform.position;
        GameObject wayPointParent = new GameObject(gameObject.name + " waypoint parent");
        GameObject westWayPointObject = new GameObject("west waypoint");
        GameObject eastWayPointObject = new GameObject("east waypoint");
        westWaypoint = westWayPointObject.transform;
        eastWaypoint = eastWayPointObject.transform;
        westWaypoint.parent = wayPointParent.transform;
        eastWaypoint.parent = wayPointParent.transform;

        westWaypoint.position = enemyPosition + (westWaypointPosition * _waypointOffsetMultiplier);
        eastWaypoint.position = enemyPosition + (eastWaypointPosition * _waypointOffsetMultiplier);
    }

    public void HandleAllMovement()
    {
        if (velocity.x < 0 && facingRight) Flip();
        else if (velocity.x > 0 && !facingRight) Flip();

        if (enemyStates.currentState == EnemyStates.State.Patrol) PatrolArea();
        if (enemyStates.currentState == EnemyStates.State.Search) { return; }
        if (enemyStates.currentState == EnemyStates.State.Chase) ChaseTarget();
        if (enemyStates.currentState == EnemyStates.State.Attack) LungeAtTarget();

        if (enemyStates.currentState == EnemyStates.State.Dead)
        {
            enemyRigidbody.isKinematic = true;
            enemyCollider.enabled = false;
            enemyRigidbody.velocity = Vector2.zero;
        }

        SetVelocity(velocity);
        Debug.Log(velocity);
    }

    private void SetVelocity(Vector2 _velocity)
    {
        enemyRigidbody.velocity = _velocity;
    }

    bool isLunging;
    public void SetLungeTrue()
    {
        Debug.Log(gameObject.name + " lunging!");
        isLunging = true;
    }
    void LungeAtTarget()
    {
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

    protected virtual void PatrolArea()
    {
        Debug.Log(gameObject.name + " patrolling");
        if (wallCheckCollider.IsTouchingLayers(enemyStates.obstaclesLayer))
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

    protected virtual void ChaseTarget()
    {
        chaseDirection = enemyStates.targetPosition - enemyRigidbody.position;
        if (chaseDirection.x > 0 && !facingRight) Flip();
        else if(chaseDirection.x < 0 && facingRight) Flip();
        velocity = chaseDirection.normalized * enemy.enemyData.speed * Time.fixedDeltaTime;
    }
    [SerializeField] GameEvent enemyLungeEvent;
    [SerializeField] UnityEvent enemyLunge;
    private void OnEnable()
    {
        enemyLungeEvent.RegisterListener(this); 
    }
    private void OnDisable()
    {
        enemyLungeEvent.UnregisterListener(this);   
    }
    public void OnEventRaised(GameEvent gameEvent)
    {
        enemyLunge?.Invoke();
    }
}
