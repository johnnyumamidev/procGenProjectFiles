using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyAttack : MonoBehaviour, IEventListener
{
    [SerializeField] EnemyAnimation enemyAnimation;
    [SerializeField] GameEvent playerDamage;
    [SerializeField] GameEvent enemyLungeEvent;
    [SerializeField] UnityEvent enemyLunge;

    Enemy enemy;
    EnemyAI enemyAi;
    public Transform attackPoint;
    [SerializeField] bool attackActive = false;
    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        enemyAi = GetComponent<EnemyAI>();
    }
    private void Start()
    {
        if (enemyAnimation == null)
        {
            enemyAnimation = GetComponentInChildren<EnemyAnimation>();
        }
        else
        {
            enemyLungeEvent = enemyAnimation.enemyLungeEvent;
        }
    }
    public void LungeAttack()
    {
        attackActive = true;
        Collider2D hit = Physics2D.OverlapCircle(attackPoint.position, enemy.enemyData.attackRadius, enemyAi.playerLayer);
        if (hit)
        {
            playerDamage.Raise();
        }
    }

    private void OnDrawGizmos()
    {
        if(enemy != null && attackActive)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, enemy.enemyData.attackRadius);
        }
    }
    private void OnEnable()
    {
        enemyLungeEvent.RegisterListener(this);
    }
    private void OnDisable()
    {
        enemyLungeEvent.UnregisterListener(this);
    }
    public void OnEventRaised(GameEvent gameEvent)
    {
        enemyLunge?.Invoke();
    }
}
