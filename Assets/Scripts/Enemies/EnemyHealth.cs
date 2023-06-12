using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyHealth : MonoBehaviour, IEventListener
{
    [SerializeField] GameEvent enemyDamageEvent;
    [SerializeField] UnityEvent enemyDamage;

    public Enemy enemy;
    EnemyAI enemyAi;
    EnemyAnimation enemyAnimation;
    int currentHealth;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        enemyAi = GetComponent<EnemyAI>();
        enemyAnimation = GetComponentInChildren<EnemyAnimation>();
        currentHealth = enemy.enemyData.maxHealth;
    }

    public void HandleHealth()
    {
        currentHealth = Mathf.Clamp(currentHealth, 0, enemy.enemyData.maxHealth);
        if (currentHealth <= 0)
        {
            enemyAi.SetEnemyState(EnemyAI.EnemyState.Dead);
            Debug.Log(gameObject.name + " died");
        }
    }
    public void TakeDamage()
    {
        if (currentHealth == 0) return;
        currentHealth--;
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
