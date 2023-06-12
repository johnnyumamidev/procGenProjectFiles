using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] GameEvent playerDamage;

    Enemy enemy;
    EnemyAI enemyAi;
    public Transform attackPoint;
    [SerializeField] bool attackActive = false;
    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        enemyAi= GetComponent<EnemyAI>();
        EventManager.instance.AddListener(gameObject.name + "lunge", LungeAttack());
    }
    public UnityAction LungeAttack()
    {
        UnityAction action = () =>
        {
            attackActive = true;
            Collider2D hit = Physics2D.OverlapCircle(attackPoint.position, enemy.enemyData.attackRadius, enemyAi.playerLayer);
            if (hit)
            {
                playerDamage.Raise();
            }
        };
        return action;
    }

    private void OnDrawGizmos()
    {
        if(enemy != null && attackActive)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, enemy.enemyData.attackRadius);
        }
    }
}
