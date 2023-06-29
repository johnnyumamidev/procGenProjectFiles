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
    public LayerMask groundLayer;
    public float groundCheckRange = 0.1f;

    Transform westWaypoint;
    Transform eastWaypoint;
    public Vector2 westWaypointPosition;
    public Vector2 eastWaypointPosition;
    [SerializeField] Transform lastWaypointReached;
    [SerializeField] protected float waypointOffsetMultiplier = 0.25f;
    public Vector2 velocity = Vector2.zero;

    protected Vector2 chaseDirection;
    public float chaseDelay;
    [SerializeField] public bool facingRight = false;

    public GameObject playerDetectedNotification;

    [SerializeField] protected bool projectilesFired = false;
    private void Awake()
    {
        enemyRigidbody = GetComponent<Rigidbody2D>();
        enemyStates = GetComponent<EnemyStates>();
        enemy = GetComponent<Enemy>();
        enemyCollider = GetComponent<CapsuleCollider2D>();

        if (enemy.enemyData.enemyType == "Flying") enemyRigidbody.gravityScale = 0;
    }

    protected bool isFiringProjectile;
    public void HandleAllMovement()
    {
        chaseDirection = enemyStates.targetPosition - enemyRigidbody.position;

        if (velocity.x < 0 && facingRight) Flip();
        else if (velocity.x > 0 && !facingRight) Flip();

        if (enemyStates.currentState == State.Patrol) PatrolArea();
        if (enemyStates.currentState == State.Search) { return; }
        if (enemyStates.currentState == State.Chase) ChaseTarget();

        if (enemyStates.currentState == State.Dead)
        {
            enemyRigidbody.isKinematic = true;
            enemyCollider.enabled = false;
            enemyRigidbody.velocity = Vector2.zero;
        }

        if(enemyStates.currentState == State.RangedAttack)
        {
            PositionForProjectile();
        }

        if (enemyStates.currentState == State.Attack)
        {
            if (!isLunging)
            {
                enemyStates.attackReady = false;
                AttackAnticipation();
            }
            else if (isLunging)
            {
                playerDetectedNotification.SetActive(false);
                Lunge();
            }
        }
        else
        {
            playerDetectedNotification.SetActive(false);
            SetVelocity(velocity);
        }
    }

    protected virtual void PositionForProjectile()
    {
        Debug.Log("preparing projectile");
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
        enemyRigidbody.AddRelativeForce(chaseDirection * enemy.enemyData.lungeForce);
        isLunging = false;
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
        if (wallCheckCollider.IsTouchingLayers(enemyStates.obstaclesLayer))
        {
            if (enemyRigidbody.velocity.x > 0) lastWaypointReached = eastWaypoint;
            else { lastWaypointReached = westWaypoint; }
        }

        if(enemy.enemyData.enemyType != "Flying")
        {
            RaycastHit2D groundCheck = Physics2D.Raycast(wallCheckCollider.transform.position, Vector2.down, groundCheckRange, groundLayer);
            if (!groundCheck)
            {
                Flip();
                if (lastWaypointReached == eastWaypoint) lastWaypointReached = westWaypoint;
                else { lastWaypointReached = eastWaypoint; }
            }
        }

        if (Vector2.Distance(transform.position, westWaypoint.position) < 0.1f) lastWaypointReached = westWaypoint;
        else if (Vector2.Distance(transform.position, eastWaypoint.position) < 0.1f) lastWaypointReached = eastWaypoint;

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
        isLunging = false;
        if (chaseDirection.x > 0 && !facingRight) Flip();
        else if(chaseDirection.x < 0 && facingRight) Flip();
        velocity = chaseDirection.normalized * enemy.enemyData.speed * Time.fixedDeltaTime;
    }
    
    // GAME EVENTS //
    [SerializeField] protected GameEvent enemyLungeEvent;
    [SerializeField] UnityEvent enemyLunge;

    [SerializeField] protected GameEvent projectileEvent;
    [SerializeField] UnityEvent fireProjectile;
    private void OnEnable()
    {
        enemyLungeEvent.RegisterListener(this);
        if(projectileEvent != null) projectileEvent.RegisterListener(this);
    }
    private void OnDisable()
    {
        enemyLungeEvent.UnregisterListener(this);
        projectileEvent?.UnregisterListener(this);
    }
    public void OnEventRaised(GameEvent gameEvent)
    {
        if (gameEvent == enemyLungeEvent) enemyLunge?.Invoke();
        if (gameEvent == projectileEvent) fireProjectile?.Invoke();
    }
    protected bool isLunging;
    public void SetLungingTrue()
    {
        isLunging = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(wallCheckCollider.transform.position, Vector2.down * groundCheckRange);
    }
}
