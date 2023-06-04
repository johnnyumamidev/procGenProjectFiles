using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    PlayerInteraction playerInteraction;
    PlayerInput playerInput;
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerInteraction = GetComponent<PlayerInteraction>();
    }

    public void HandleAllAttackActions()
    {
        HandlePlayerAttack();
    }

    private void HandlePlayerAttack()
    {
        if (!playerInteraction.currentlyHoldingItem) return;
        if (playerInput.performAttack != 0) Debug.Log("attack");
    } 
}
