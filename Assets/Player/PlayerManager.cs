using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    PlayerLocomotion playerLocomotion;
    PlayerHealth playerHealth;
    PlayerAttack playerAttack;
    public PlayerInput playerInput;
    public PlayerInventory playerInventory;
    PlayerInteraction playerInteraction;

    void Start()
    {
        if(UIManager.instance != null) UIManager.instance.playerManager = this;
        playerInteraction = GetComponent<PlayerInteraction>();
        playerInput = GetComponent<PlayerInput>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        playerHealth = GetComponent<PlayerHealth>();
        playerAttack = GetComponent<PlayerAttack>();
        playerInventory = GetComponent<PlayerInventory>();
    }

    // Update is called once per frame
    void Update()
    {
        playerInventory.HandleInventory();
        playerInteraction.HandleInteraction();
        playerHealth.HandleHealth();
        if (GameStateManager.instance != null && GameStateManager.instance.currentState == "Game Over") return;
        playerAttack.HandleAllAttackActions();
        playerInput.HandleAllInputs();
    }

    private void FixedUpdate()
    {
        if (GameStateManager.instance != null && GameStateManager.instance.currentState == "Game Over") return;
        if (playerAttack.isMeleeAttacking) return;
        playerLocomotion.HandleAllMovement();   
    }
}
