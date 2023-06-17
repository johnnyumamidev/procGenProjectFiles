using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GargoyleAI : EnemyAI
{
    public float gargoylePatrolModifier;

    public float flightHeight = 2.5f;

    [SerializeField] GameEvent gargoyleProjectileEvent;
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
            projectilesFired = false;
            if (enemyStates.targetPosition.x > enemyRigidbody.position.x && !facingRight) Flip();
            else if (enemyStates.targetPosition.x < enemyRigidbody.position.x && facingRight) Flip();
        }
        velocity = new Vector2(xVelocity, yVelocity) * enemy.enemyData.speed * Time.fixedDeltaTime;
    }
    
    bool hasRolledForRangedPosition = false;
    bool projectilesFired = false;
    int index = 0;

    public List<Transform> rangedAttackPositions = new List<Transform>();
    protected override void PositionForProjectile()
    {
        if (!hasRolledForRangedPosition)
        {
            index = GetRandomIndex();
        }
        Vector2 rangedPosition = rangedAttackPositions[index].position;
        Vector2 directionToPosition = rangedPosition - enemyRigidbody.position;

        if (Vector2.Distance(enemyRigidbody.position, rangedPosition) > 0.05f)
        {
            velocity = new Vector2(directionToPosition.normalized.x, 0) * enemy.enemyData.speed * Time.fixedDeltaTime;
        }
        else
        {
            velocity = Vector2.zero;
            if (enemyStates.targetPosition.x > enemyRigidbody.position.x && !facingRight) Flip();
            else if (enemyStates.targetPosition.x < enemyRigidbody.position.x && facingRight) Flip();
            if (projectilesFired) return;
            gargoyleProjectileEvent.Raise();
            projectilesFired = true;
        }
    }

    private int GetRandomIndex()
    {
        int randomIndex = Random.Range(0, rangedAttackPositions.Count - 1);
        hasRolledForRangedPosition = true;
        return randomIndex;
    }

    protected override void Lunge()
    {
        enemyStates.attackReady = false;
        enemyRigidbody.AddForce(Vector2.down * enemy.enemyData.lungeForce);
    }
}
