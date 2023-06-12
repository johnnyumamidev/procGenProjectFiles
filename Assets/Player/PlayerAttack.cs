using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] GameEvent attackActive;
    [SerializeField] GameEvent attackInactive;

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
        if (playerInteraction.currentlyHeldItem.GetType().ToString() != "Weapon") return;

        if (playerInput.performAttack != 0)
        {
            attackActive.Raise();
        }
        else
        {
            attackInactive.Raise();
        }
    } 
}
