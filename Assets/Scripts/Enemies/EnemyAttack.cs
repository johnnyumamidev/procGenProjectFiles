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
    [SerializeField] UnityEvent fireProjectile;
    Enemy enemy;
    EnemyStates enemyStates;
    public Transform attackPoint;

    [SerializeField] GameObject enemyProjectilePrefab;
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

    List<GameObject> projectiles = new List<GameObject>();
    List<GameObject> bullets = new List<GameObject>();
    public int numberOfProjectiles = 5;
    public float bulletSpeed = 20f;
    public Transform firingPoint;

    public void FireProjectile()
    {
        Debug.Log(gameObject.name + " firing projectile");

        if(projectiles.Count < numberOfProjectiles)
        {
            GameObject bullet = Instantiate(enemyProjectilePrefab, firingPoint.position, Quaternion.identity);
            bullets.Add(bullet);

            Rigidbody2D bulletRigidbody = bullet.GetComponent<Rigidbody2D>();
            Vector2 bulletDirection = firingPoint.position - transform.position;
            Vector2 _velocity = bulletDirection.normalized * bulletSpeed * Time.fixedDeltaTime;
            bulletRigidbody.velocity = _velocity;
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
        projectileEvent.RegisterListener(this);
    }
    private void OnDisable()
    {
        enemyLungeEvent.UnregisterListener(this);
        projectileEvent.UnregisterListener(this);
    }
    public void OnEventRaised(GameEvent gameEvent)
    {
        if(gameEvent == enemyLungeEvent)enemyLunge?.Invoke();
        if(gameEvent == projectileEvent) fireProjectile?.Invoke();
    }
}
