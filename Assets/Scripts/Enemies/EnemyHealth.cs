using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyHealth : MonoBehaviour
{
    public Enemy enemy;
    EnemyAnimation enemyAnimation;
    int currentHealth;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        enemyAnimation = GetComponent<EnemyAnimation>();
        currentHealth = enemy.enemyData.maxHealth;
        EventManager.instance.AddListener(gameObject.name + "_take_damage", TakeDamage());
    }

    public void HandleHealth()
    {
        currentHealth = Mathf.Clamp(currentHealth, 0, enemy.enemyData.maxHealth);
        if(currentHealth <= 0) { Debug.Log(gameObject.name + " died"); }
    }

    private UnityAction TakeDamage()
    {
        UnityAction action = () =>
        {
            currentHealth--;
            enemyAnimation.animator.Play("EnemyHurt");
            Debug.Log(gameObject.name + "took damage, health remaining: " + currentHealth);
        };
        return action;
    }
}
