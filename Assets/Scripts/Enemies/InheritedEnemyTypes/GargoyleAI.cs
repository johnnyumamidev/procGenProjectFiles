using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GargoyleAI : EnemyAI
{
    public float gargoylePatrolModifier;

    public float flightHeight = 2.5f;

    void Start()
    {
        SpawnPatrolPoints(gargoylePatrolModifier);
    }

    protected override void ChaseTarget()
    {
        isLunging = false;
        chaseDirection = enemyStates.targetPosition - enemyRigidbody.position;
        float yVelocity = 0;
        float xVelocity = chaseDirection.normalized.x;
        if (enemyRigidbody.position.y < flightHeight)
        {
            xVelocity = 0;
            yVelocity = 1;
            enemyStates.attackReady = false;
        }
        else
        {
            enemyStates.attackReady = true;
            hasRolledForRangedPosition = false;
            if (enemyStates.targetPosition.x > enemyRigidbody.position.x && !facingRight) Flip();
            else if (enemyStates.targetPosition.x < enemyRigidbody.position.x && facingRight) Flip();
        }
        velocity = new Vector2(xVelocity, yVelocity) * enemy.enemyData.speed * Time.fixedDeltaTime;
    }

    [SerializeField] bool hasRolledForRangedPosition = false;
    int index = 0;

    public List<Transform> rangedAttackPositions = new List<Transform>();
    Transform lastPositionChosen;
    protected override void PositionForProjectile()
    {
        if (!hasRolledForRangedPosition)
        {
        RollRandomPosition:
            index = GetRandomIndex();

            if (lastPositionChosen != null && rangedAttackPositions[index] == lastPositionChosen)
            {
                goto RollRandomPosition;
            }
            hasRolledForRangedPosition = true;
        }
        lastPositionChosen = rangedAttackPositions[index];
        Vector2 rangedPosition = rangedAttackPositions[index].position;
        Vector2 directionToPosition = rangedPosition - enemyRigidbody.position;

        if (Vector2.Distance(enemyRigidbody.position, rangedPosition) > 0.01f)
        {
            enemyStates.attackReady = false;
            velocity = new Vector2(directionToPosition.normalized.x, 0) * enemy.enemyData.speed * Time.fixedDeltaTime;
        }
        else
        {
            enemyStates.attackReady = true;
            velocity = Vector2.zero;
            if (enemyStates.targetPosition.x > enemyRigidbody.position.x && !facingRight) Flip();
            else if (enemyStates.targetPosition.x < enemyRigidbody.position.x && facingRight) Flip();
            if (!enemyStates.attackReady) return;
            else if (!projectilesFired)
            {
                projectileEvent.Raise();
                StartCoroutine(ResetProjectilesFired());
            }
        }
    }

    public float projectileCooldownTime = 1f;
    public void ProjectilesFired()
    {
        projectilesFired = true;
    }
    public IEnumerator ResetProjectilesFired()
    {
        while (projectilesFired)
        {
            yield return new WaitForSeconds(projectileCooldownTime);
            projectilesFired = false;
        }
    }
    private int GetRandomIndex()
    {
        int randomIndex = Random.Range(0, rangedAttackPositions.Count - 1);
        return randomIndex;
    }

    protected override void Lunge()
    {
        enemyRigidbody.AddForce(Vector2.down * enemy.enemyData.lungeForce);
    }
}
