using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStates : MonoBehaviour
{
    Enemy enemy;
    EnemyAI enemyAi;
    public State currentState;
    public LayerMask playerLayer;
    public LayerMask obstaclesLayer;

    public Vector2 targetPosition;

    public Transform attackHitbox;

    public bool attackReady = true;
    public int chanceToFireProjecile = 5;
    public enum State
    {
        Patrol, Chase, Attack, Search, Dead, RangedAttack
    }
    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        enemyAi = GetComponent<EnemyAI>();
    }
    private void Start()
    {
        currentState = State.Patrol;
        SetEnemyState(currentState);
    }

    public void HandleStates()
    {
        if (currentState == State.Dead) return;
        if (enemy.enemyData.hasRangedWeapon) HandleRandomRangedAttack();
        DetectPlayerSphere();
        if (currentState == State.RangedAttack) return;
        else
        {
            CheckTargetWithinAttackRange();
        }
    }
    bool hasRolled;
    public float rollCooldownTime = 3f;
    float rollCooldown;
    private void HandleRandomRangedAttack()
    {
        if (!hasRolled) rollCooldown = rollCooldownTime;
        if (currentState == State.Chase && !hasRolled && attackReady)
        {
            int randomRoll = Random.Range(0, 10);
            hasRolled = true;
            if (randomRoll >= chanceToFireProjecile)
            {
                Debug.Log(randomRoll + ", roll succeeded! firing projectile!");
                currentState = State.RangedAttack;
            }
            else
            {
                Debug.Log(randomRoll + ", roll failed! will not fire projectile");
                currentState = State.Chase;
            }
        }

        if (hasRolled && rollCooldown > 0)
            rollCooldown -= Time.deltaTime;
        else { hasRolled = false; }
    }

        public void SetEnemyState(State newState)
    {
        currentState = newState;
    }

    private void CheckTargetWithinAttackRange()
    {
        Collider2D attackRange = Physics2D.OverlapCircle(attackHitbox.position, enemy.enemyData.attackRange, playerLayer);
        if (attackRange && currentState != State.RangedAttack)
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

            if (currentState == State.RangedAttack) return;
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

    public void ReadyAttack()
    {
        attackReady = true;
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
