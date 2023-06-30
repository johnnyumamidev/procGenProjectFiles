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
    PlayerHealth playerHealth;

    public int currentHealth;
    public ArmorState[] armorState;
    [SerializeField] GameObject spriteObject;
    [SerializeField] Transform weaponPosition;
    SpriteRenderer spriteRenderer;

    public int animStateIndex = 0;
    public List<string> animStates = new List<string> { "Idle", "Run", "Jump", "Falling" };

    public Transform weaponHolder;
    private void Awake()
    {
        spriteRenderer = spriteObject.GetComponent<SpriteRenderer>();
        animator = spriteObject.GetComponent<Animator>();

        playerHealth = GetComponentInParent<PlayerHealth>();
        if (playerInput == null) playerInput = GetComponentInParent<PlayerInput>();
        if (playerLocomotion == null) playerLocomotion = GetComponentInParent<PlayerLocomotion>();
    }

    private void LateUpdate()
    {
        if (spriteRenderer.sprite.name.Contains("fullArmor"))
        {
            string spriteName = spriteRenderer.sprite.name;
            spriteName = spriteName.Replace("fullArmor_","");
            int spriteNumber = int.Parse(spriteName);

            spriteRenderer.sprite = armorState[currentHealth-1].sprites[spriteNumber];
        }
    }

    private void Update()
    {
        currentHealth = (int)playerHealth.currentHealth;
        weaponHolder.transform.position = weaponPosition.position;

        SetAnimationBasedOnPlayerState();

        animStateIndex = Mathf.Clamp(animStateIndex, 0, animStates.Count-1);
        animator.CrossFade(animStates[animStateIndex], 0, 0);
    }

    private void SetAnimationBasedOnPlayerState()
    {
        if (playerLocomotion.isGrounded && playerInput.movementInput.x != 0) { animStateIndex = 8; }
        else if (!playerLocomotion.isGrounded && !playerLocomotion.isNearChain) { animStateIndex = 2; }
        else if (playerLocomotion.isNearChain && playerInput.movementInput == Vector2.zero && playerLocomotion.isGrounded) { animStateIndex = 4; }
        else if (playerLocomotion.isClimbing && !playerLocomotion.isGrounded) { animStateIndex = 5; }
        else if (!playerLocomotion.isGrounded && playerLocomotion.isNearChain && !playerLocomotion.isClimbing) { animStateIndex = 6; }
        else { animStateIndex = 0; }

        if (playerLocomotion.isDodging)
        {
            animStateIndex = 9;
            return;
        }
        if (playerLocomotion.isClimbing)
        {
            animStateIndex = 5;
            return;
        }

        if (playerInput.performAttack != 0) animStateIndex = 7;

        if (playerHealth.playerHurtState) animStateIndex = 10;
        if (playerHealth.currentHealth == 0) animStateIndex = 11;
    }

    [System.Serializable]
    public struct ArmorState
    {
        public Sprite[] sprites;
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
