using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GargoyleAI : EnemyAI
{
    public float gargoylePatrolModifier;
    public float attackStartRange = 0.5f;

    public float flyingHeight;
    public float flightSpeed;
    void Start()
    {
        SpawnPatrolPoints(gargoylePatrolModifier);
    }

    protected override void ChaseTarget()
    {
        chaseDirection = enemyStates.targetPosition - enemyRigidbody.position;
        velocity = new Vector2(chaseDirection.normalized.x, 0) * enemy.enemyData.speed * Time.fixedDeltaTime;
        
        if(chaseDirection.x < attackStartRange)
        {
            enemyStates.SetEnemyState(EnemyStates.State.Attack);
        }
        else
        {
            enemyStates.SetEnemyState(EnemyStates.State.Chase);
        }
    }

    protected override void AttackAnticipation()
    {
        if (enemyCollider.IsTouchingLayers(enemyStates.obstaclesLayer))
        {
            Debug.Log("gargoyle has slammed onto ground");
            StartCoroutine(FlyToSlamPosition());
        }
        else
        {
            playerDetectedNotification.SetActive(true);
            SetVelocity(Vector2.zero);
        }
    }

    public float slamAttackCooldown = 0.5f;
    private IEnumerator FlyToSlamPosition()
    {
        yield return new WaitForSeconds(slamAttackCooldown);
        Debug.Log("go back to attack start position");
        while(enemyRigidbody.position.y < flyingHeight)
        {
            enemyRigidbody.AddRelativeForce(Vector2.up);
        }
    }
    protected override void Lunge()
    {
        Debug.Log("gargoyle slam!");
        Vector2 direction = Vector2.down;
        enemyRigidbody.AddRelativeForce(direction * enemy.enemyData.lungeForce);
    }
}
