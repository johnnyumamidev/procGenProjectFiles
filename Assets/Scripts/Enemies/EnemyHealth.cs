using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyHealth : MonoBehaviour, IEventListener
{
    public GameEvent enemyDamageEvent;
    [SerializeField] UnityEvent enemyDamage;

    public Enemy enemy;
    EnemyStates enemyStates;
    EnemyAnimation enemyAnimation;
    int currentHealth;
    public GameObject bloodParticles;

    bool deathEvent;

    private void Awake()
    {
        if(enemyDamageEvent == null) enemyDamageEvent = new GameEvent();
        enemyDamageEvent.name = gameObject.name + " damage";
        enemy = GetComponent<Enemy>();
        enemyStates = GetComponent<EnemyStates>();
        enemyAnimation = GetComponentInChildren<EnemyAnimation>();
        currentHealth = enemy.enemyData.maxHealth;
    }

    public void HandleHealth()
    {
        currentHealth = Mathf.Clamp(currentHealth, 0, enemy.enemyData.maxHealth);
        if (currentHealth <= 0 && !deathEvent)
        {
            Instantiate(bloodParticles, transform.position, Quaternion.identity);
            enemyStates.SetEnemyState(EnemyStates.State.Dead);
            deathEvent = true;
        }
    }
    public void TakeDamage()
    {
        if (currentHealth == 0) return;
        currentHealth--;
        Debug.Log(gameObject.name + " damaged");
        enemyAnimation.animator.Play("EnemyHurt");
    }
    private void OnEnable()
    {
        enemyDamageEvent.RegisterListener(this);
    }
    private void OnDisable()
    {
        enemyDamageEvent.UnregisterListener(this);
    }
    public void OnEventRaised(GameEvent gameEvent)
    {
        enemyDamage?.Invoke();
    }
}
