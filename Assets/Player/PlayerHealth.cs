using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    public PlayerData playerData;
    float maxHealth;
    public float currentHealth;
    public bool playerHurtState = false;
    bool isInvincible;
    [SerializeField] float hurtTimer;

    public GameObject bloodParticles;

    void Awake()
    {
        maxHealth = playerData.maxPlayerHealth;
        SetHealthToFull();
        EventManager.instance.AddListener("damage", TakeDamage());
        EventManager.instance.AddListener("reset_health", ResetHealth());
        EventManager.instance.AddListener("retry", Retry());
    }

    private void SetHealthToFull()
    {
        currentHealth = maxHealth;
    }

    public void HandleHealth()
    {
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        HandleHurtState();
        HandlePlayerDeath();
    }

    private void HandlePlayerDeath()
    {
        if (currentHealth <= 0)
        {
            isInvincible = true;
            EventManager.instance.TriggerEvent("game_over");
        }
    }

    private void HandleHurtState()
    {
        isInvincible = false;
        if (playerHurtState)
        {
            isInvincible = true;
            bloodParticles.SetActive(true);
            hurtTimer += Time.deltaTime;
        }
        else { hurtTimer = 0; }
        if (hurtTimer >= playerData.hurtStateTime) playerHurtState = false;
    }

    private UnityAction ResetHealth()
    {
        UnityAction action = () => { currentHealth = maxHealth; };
        return action;
    }

    private UnityAction TakeDamage()
    {
        UnityAction action = () =>
        {
            if (!isInvincible)
            {
                currentHealth--;
                playerHurtState = true;
                EventManager.instance.TriggerEvent("update_animator");
                EventManager.instance.TriggerEvent("camera_shake");
                Debug.Log("damage taken, total health: " + currentHealth);
            }
        };
        return action;
    }

    private UnityAction Retry()
    {
        UnityAction action = () =>
        {
            SetHealthToFull();
            EventManager.instance.TriggerEvent("update_animator");
        };
        return action;
    }
}
