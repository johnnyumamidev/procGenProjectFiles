using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

public class EnemyAttack : MonoBehaviour, IEventListener
{
    [SerializeField] EnemyAnimation enemyAnimation;
    [SerializeField] GameEvent playerDamage;
    [SerializeField] GameEvent enemyLungeEvent;
    [SerializeField] UnityEvent enemyLunge;
    [SerializeField] GameEvent projectileEvent;
    Enemy enemy;
    EnemyAI enemyAI;
    EnemyStates enemyStates;
    public Transform attackPoint;

    [SerializeField] GameObject enemyProjectilePrefab;
    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        enemyAI = GetComponent<EnemyAI>();
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

    List<GameObject> bullets = new List<GameObject>();
    public float bulletSpeed = 20f;
    public int bulletSpread;
    public float angleMultiplier = 2;
    public Transform firingPoint;
    public Transform reticle;
    public float bulletSpreadAngle;
    public void FireProjectile()
    {
        Debug.Log(gameObject.name + " firing projectile");
        for (int i = 0; i < bulletSpread; i++)
        {
            
            GameObject bullet = Instantiate(enemyProjectilePrefab, firingPoint.position, Quaternion.identity);
            bullets.Add(bullet);

            Vector2 centerVector = reticle.position - firingPoint.position;
            float centerVectorAngle= Mathf.Atan2(centerVector.x, centerVector.y);
            float remainingAngle = Mathf.PI / 2 - centerVectorAngle;
            bulletSpreadAngle = (centerVectorAngle + remainingAngle) * angleMultiplier;
            float angleStep = (bulletSpreadAngle / bulletSpread) * i;
           
            EnemyProjectileBehavior projectileBehavior = bullet.GetComponent<EnemyProjectileBehavior>();
            projectileBehavior.SetVelocity(centerVectorAngle, angleStep, enemyAI.facingRight);
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
        if(gameEvent == enemyLungeEvent)enemyLunge?.Invoke();
    }
}
