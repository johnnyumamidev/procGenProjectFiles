using Cinemachine.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAnimationManager : MonoBehaviour, IEventListener
{
    Animator animator;
    PlayerInput playerInput;
    PlayerLocomotion playerLocomotion;
    PlayerInteraction playerInteraction;
    PlayerHealth playerHealth;

    public PlayerData playerData;
    
    public int animStateIndex;
    public List<string> animStates = new List<string> { "Idle", "Run", "Jump", "Falling" };
    public List<GameObject> armorState = new List<GameObject>();
    [SerializeField] int armorIndex = 3;

    public Transform weaponHolder;
    private void Awake()
    {
        playerInteraction = GetComponentInParent<PlayerInteraction>();
        playerHealth = GetComponentInParent<PlayerHealth>();
        if (playerInput == null) playerInput = GetComponentInParent<PlayerInput>();
        if (playerLocomotion == null) playerLocomotion = GetComponentInParent<PlayerLocomotion>();
    }

    public void UpdateAnimator()
    {
        if (armorIndex > 0)
        {
            animator.gameObject.SetActive(false);
            armorIndex--;
            armorState[armorIndex].SetActive(true);
        }
    }

    public void ResetAnimator()
    {
        animator.gameObject.SetActive(false);
        armorIndex = 3;
        armorState[armorIndex].SetActive(true);
    }

    private void Start()
    {
        animStateIndex = 0;
    }

    private void Update()
    {
        armorIndex = Mathf.Clamp(armorIndex, 0, armorState.Count - 1);
        animator = armorState[armorIndex].GetComponent<Animator>();
        Transform weapon = armorState[armorIndex].GetComponentInChildren<Transform>();
        if(weapon.CompareTag("Weapon")) weaponHolder.transform.position = weapon.position;

        HandlePlayerInput();

        animStateIndex = Mathf.Clamp(animStateIndex, 0, animStates.Count-1);
        animator.CrossFade(animStates[animStateIndex], 0, 0);
    }

    private void HandlePlayerInput()
    {
        if (playerLocomotion.isGrounded && playerInput.movementInput.x != 0) { animStateIndex = 1; }
        else if (!playerLocomotion.isGrounded && !playerLocomotion.isNearChain) { animStateIndex = 2; }
        else if (playerLocomotion.isNearChain && playerInput.movementInput == Vector2.zero && playerLocomotion.isGrounded) { animStateIndex = 4; }
        else if (playerLocomotion.isClimbing && !playerLocomotion.isGrounded) { animStateIndex = 5; }
        else if (!playerLocomotion.isGrounded && playerLocomotion.isNearChain && !playerLocomotion.isClimbing) { animStateIndex = 6; }
        else { animStateIndex = 0; }
        
        if (playerInteraction.currentlyHoldingItem)
        {
            var itemType = playerInteraction.currentlyHeldItem.GetType();
            if (itemType.ToString() == "Weapon")
            {
                if (playerInput.performAttack != 0) animStateIndex = 12;
                if (playerInput.movementInput.x != 0) animStateIndex = 13;
                return;
            }
            if (playerInput.movementInput == Vector2.zero) animStateIndex = 7;
            else if (playerInput.movementInput.x != 0) animStateIndex = 8;
            
            if (!playerLocomotion.isGrounded) animStateIndex = 9;
        }

        if (playerHealth.playerHurtState) animStateIndex = 10;
        if (playerHealth.currentHealth == 0) animStateIndex = 11;
        if (playerLocomotion.isDodging) animStateIndex = 15;
    }

    [SerializeField] GameEvent playerSpawnEvent;
    [SerializeField] UnityEvent playerSpawned;
    private void OnEnable()
    {
        playerSpawnEvent.RegisterListener(this);
    }
    private void OnDisable()
    {
        playerSpawnEvent.UnregisterListener(this);
    }
    public void OnEventRaised(GameEvent gameEvent)
    {
        playerSpawned?.Invoke();
    }
}
