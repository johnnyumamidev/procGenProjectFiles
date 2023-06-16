using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GargoyleAI : EnemyAI
{
    public float gargoylePatrolModifier;
    bool attackComplete = true;
   
    void Start()
    {
        SpawnPatrolPoints(gargoylePatrolModifier);
    }

    protected override void PatrolArea()
    {
        //custom gargoyle movement
        Debug.Log("custom gargoyle movement");
    }

    protected override void Lunge()
    {
        Debug.Log("gargoyle slam!");
        Vector2 direction = Vector2.down;
        enemyRigidbody.AddRelativeForce(direction * enemy.enemyData.lungeForce);
    }
}
