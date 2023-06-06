using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Enemy/Data")]
public class EnemyData : ScriptableObject
{
    public string enemyType;

    [Header("Movement")]
    public float speed;

    [Header("Health")]
    public int maxHealth;

    [Header("AI/Detection")]
    public float detectPlayerRadius;
}
