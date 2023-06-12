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

    void Start()
    {
        UIManager.instance.playerManager = this;
        playerInput = GetComponent<PlayerInput>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        playerHealth = GetComponent<PlayerHealth>();
        playerAttack = GetComponent<PlayerAttack>();
        playerInventory = GetComponent<PlayerInventory>();
    }

    // Update is called once per frame
    void Update()
    {
        playerHealth.HandleHealth();
        if (GameStateManager.instance.currentState == "Game Over") return;
        playerInput.HandleAllInputs();
        playerAttack.HandleAllAttackActions();
        playerInventory.HandleInventory();
    }

    private void FixedUpdate()
    {
        if (GameStateManager.instance.currentState == "Game Over") return;
        playerLocomotion.HandleAllMovement();   
    }
}
