using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatAI : EnemyAI
{
    // Start is called before the first frame update
    public float waypointMultiplier = 1.5f;
    void Start()
    {
        SpawnPatrolPoints(waypointMultiplier);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
