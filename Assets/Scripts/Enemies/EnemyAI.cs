using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static EnemyStates;

public class EnemyAI : MonoBehaviour, IEventListener
{
    protected Enemy enemy;
    protected EnemyStates enemyStates;
    protected Rigidbody2D enemyRigidbody;
    protected CapsuleCollider2D enemyCollider;
    public CircleCollider2D wallCheckCollider;

    Transform westWaypoint;
    Transform eastWaypoint;
    public Vector2 westWaypointPosition;
    public Vector2 eastWaypointPosition;
    [SerializeField] Transform lastWaypointReached;
    [SerializeField] protected float waypointOffsetMultiplier = 0.25f;
    public Vector2 velocity = Vector2.zero;

    protected Vector2 chaseDirection;
    public float chaseDelay;
    bool facingRight = false;

    public GameObject playerDetectedNotification;
    private void Awake()
    {
        enemyRigidbody = GetComponent<Rigidbody2D>();
        enemyStates = GetComponent<EnemyStates>();
        enemy = GetComponent<Enemy>();
        enemyCollider = GetComponent<CapsuleCollider2D>();

        if (enemy.enemyData.enemyType == "Flying") enemyRigidbody.gravityScale = 0;
    }
    public void HandleAllMovement()
    {
        if (velocity.x < 0 && facingRight) Flip();
        else if (velocity.x > 0 && !facingRight) Flip();

        if (enemyStates.currentState == EnemyStates.State.Patrol) PatrolArea();
        if (enemyStates.currentState == EnemyStates.State.Search) { return; }
        if (enemyStates.currentState == EnemyStates.State.Chase) ChaseTarget();

        if (enemyStates.currentState == EnemyStates.State.Dead)
        {
            enemyRigidbody.isKinematic = true;
            enemyCollider.enabled = false;
            enemyRigidbody.velocity = Vector2.zero;
        }

        if (enemyStates.currentState == EnemyStates.State.Attack)
        {
            if (!isLunging)
            {
                Debug.Log("preparing attack!");
                AttackAnticipation();
            }
            else
            {
                playerDetectedNotification.SetActive(false);
                Lunge();
                isLunging = false;
            }
        }
        else
        {
            playerDetectedNotification.SetActive(false);
            Debug.Log(gameObject.name + " not currently attacking");
            SetVelocity(velocity);
        }
    }

    protected virtual void AttackAnticipation()
    {
        playerDetectedNotification.SetActive(true);
        SetVelocity(Vector2.zero);
    }

    // === PROTECTED/VIRTUAL FUNCTIONS === //
    // may get overridden by child classes that inherit from enemyAI //

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

    protected void SetVelocity(Vector2 _velocity)
    {
        enemyRigidbody.velocity = _velocity;
    }
    protected virtual void Lunge()
    {
        Vector2 direction = Vector2.right;
        if (!facingRight) direction = Vector2.left;
        enemyRigidbody.AddRelativeForce(direction * enemy.enemyData.lungeForce);
    }
    protected void Flip()
    {
        facingRight = !facingRight;
        float xScale = transform.localScale.x;
        xScale *= -1;
        transform.localScale = new Vector3(xScale, transform.localScale.y, 0);
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
        if (lastWaypointReached == null)
        {
            velocity = Vector2.right * enemy.enemyData.speed * Time.fixedDeltaTime;
        }
        else
        {
            if (lastWaypointReached == eastWaypoint) velocity = Vector2.left * enemy.enemyData.speed * Time.fixedDeltaTime;
            else if (lastWaypointReached == westWaypoint) velocity = Vector2.right * enemy.enemyData.speed * Time.fixedDeltaTime;
        }
    }

    protected virtual void ChaseTarget()
    {
        chaseDirection = enemyStates.targetPosition - enemyRigidbody.position;
        if (chaseDirection.x > 0 && !facingRight) Flip();
        else if(chaseDirection.x < 0 && facingRight) Flip();
        velocity = chaseDirection.normalized * enemy.enemyData.speed * Time.fixedDeltaTime;
    }
    
    // GAME EVENTS //
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
    bool isLunging;
    public void SetLungingTrue()
    {
        isLunging = true;
    }
}
