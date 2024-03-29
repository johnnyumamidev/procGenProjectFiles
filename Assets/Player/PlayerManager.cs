using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    PlayerInput playerInput;
    PlayerLocomotion playerLocomotion;
    PlayerHealth playerHealth;
    PlayerAttack playerAttack;
    public PlayerInventory playerInventory;

    private void OnEnable()
    {
        UIManager.instance.playerManager = this;
    }
    void Start()
    {
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
        playerLocomotion.HandleAllMovement();   
    }
}
