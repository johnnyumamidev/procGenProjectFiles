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
    EnemyStates enemyStates;
    public Transform attackPoint;
    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        enemyStates = GetComponent<EnemyStates>();
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
        Collider2D hit = Physics2D.OverlapCircle(attackPoint.position, enemy.enemyData.attackRadius, enemyStates.playerLayer);
        if (hit)
        {
            playerDamage.Raise();
        }
    }

    private void OnDrawGizmos()
    {
        if(enemy != null)
        {
            Gizmos.color = Color.blue;
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
