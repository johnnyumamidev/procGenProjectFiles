using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        }
        velocity = new Vector2(xVelocity, yVelocity) * enemy.enemyData.speed * Time.fixedDeltaTime;
    }

    protected override void Lunge()
    {
        enemyRigidbody.AddForce(Vector2.down * enemy.enemyData.lungeForce);
    }
}
