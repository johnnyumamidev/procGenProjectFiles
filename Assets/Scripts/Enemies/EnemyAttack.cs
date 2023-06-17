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
    public Transform firingPoint;
    public void FireProjectile()
    {
        Debug.Log(gameObject.name + " firing projectile");
        for (int i = 0; i < bulletSpread; i++)
        {
            GameObject bullet = Instantiate(enemyProjectilePrefab, firingPoint.position, Quaternion.identity);
            bullets.Add(bullet);

            float theta = Mathf.Atan2(firingPoint.position.x, firingPoint.position.y);
            float spreadRadians = Mathf.PI * bulletSpread / 30f;

            float bulletSpreadMultiplier = spreadRadians * i;

            Rigidbody2D bulletRigidbody = bullet.GetComponent<Rigidbody2D>();
            Vector2 _velocity = new Vector2(Mathf.Cos(theta - bulletSpreadMultiplier), Mathf.Sin(theta - bulletSpreadMultiplier));
            if (!enemyAI.facingRight) _velocity *= -1;
            bulletRigidbody.velocity = _velocity;

            float reticlePosInDegrees = Mathf.Rad2Deg * theta;
            bullet.transform.rotation = Quaternion.Euler(0, 0, reticlePosInDegrees - 90);
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
