using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Enemy/Data")]
public class EnemyData : ScriptableObject
{
    public string enemyType;
    public bool hasRangedWeapon;

    [Header("Movement")]
    public float speed;

    [Header("Health")]
    public int maxHealth;

    [Header("AI/Detection")]
    public float detectPlayerRadius;
    public float attackRange;

    [Header("Attack")]
    public float attackSpeed;
    public float attackRadius = 0.05f;
    public float lungeForce = 10f;
}
