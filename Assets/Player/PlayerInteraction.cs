using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static UnityEditor.Progress;

public class PlayerInteraction : MonoBehaviour, IEventListener
{
    public AudioSource audioSource;
    public AudioClip pickUpSFX;

    public float interactionCooldown = 1f;
    float interactionTimer;
    bool interactionReady = true;

    public GameObject pointer;
    public Vector2 pointerOffset;

    public Transform interactionHitbox;
    public Transform weaponHoldPosition;

    public float interactionRadius = 1f;
    [SerializeField]
    private LayerMask interactionLayerMask;

    //component references
    PlayerInput playerInput;
    PlayerHealth playerHealth;
    PlayerLocomotion playerLocomotion;
    private readonly Collider2D[] colliders = new Collider2D[2];
    [SerializeField] private int numberFound;

    public bool currentlyHoldingItem = false;
    public IInteractable currentlyHeldItem;
    public GameObject itemObject;

    public bool hasKey = false;

    bool npcDetected;
    NPC currentNPC;
    private void Awake()
    {
        interactionTimer = interactionCooldown;
        playerInput = GetComponent<PlayerInput>();
        playerHealth = GetComponent<PlayerHealth>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
    }

    void Update()
    {
        npcDetected = false;
        pointer.SetActive(false);
        if (!playerLocomotion.isGrounded) return;

        if (!interactionReady)
        {
            interactionTimer -= Time.deltaTime;
        }

        if (interactionTimer <= 0)
        {
            interactionReady = true;
            interactionTimer = interactionCooldown;
        }

        numberFound = Physics2D.OverlapCircleNonAlloc(interactionHitbox.position, interactionRadius, colliders, interactionLayerMask);

        if (numberFound > 0 && interactionReady)
        {
            itemObject = colliders[0].gameObject;
            var interactable = itemObject.GetComponent<IInteractable>();
            Vector2 objectPosition = colliders[0].transform.position;
            pointer.SetActive(true);
            pointer.transform.position = objectPosition + pointerOffset;

            if (!currentlyHoldingItem && interactable != null && playerInput.performInteract != 0)
            {
                interactable.Interact(this);
                interactionReady = false;

                if (interactable.interactableObject.CompareTag("Holdable"))
                {
                    audioSource.clip = pickUpSFX;
                    audioSource.Play();
                    currentlyHoldingItem = true;
                    currentlyHeldItem = interactable;
                    Debug.Log("current item: " + currentlyHeldItem.ToString());
                }
            }

            if (colliders[0].CompareTag("NPC"))
            {
                npcDetected = true;
                currentNPC = colliders[0].GetComponent<NPC>();
            }
        }

        if (playerInput.performInteract != 0 && interactionReady)
        {
            if (currentlyHoldingItem)
            {
                audioSource.clip = pickUpSFX;
                audioSource.Play();
                DropCurrentlyHeldItem();
                interactionReady = false;
            }

            if (npcDetected)
            {
                if (currentNPC != null) currentNPC.NPCAction();
            }
        }

        if(currentlyHoldingItem) { pointer.SetActive(false); }
    }

    private void DropCurrentlyHeldItem()
    {
        Debug.Log("dropping current item: " + currentlyHeldItem.ToString());
        currentlyHeldItem.DropItem();
        currentlyHoldingItem = false;
        currentlyHeldItem = null;
    }

    public void KeyCollected()
    {
        hasKey = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(interactionHitbox.position, interactionRadius);
    }

    [SerializeField] GameEvent unlockEvent;
    [SerializeField] UnityEvent unlock;
    [SerializeField] GameEvent keyTakenEvent;
    [SerializeField] UnityEvent keyTaken;

    private void OnEnable()
    {
        unlockEvent.RegisterListener(this);
        keyTakenEvent.RegisterListener(this);
    }
    private void OnDisable()
    {
        unlockEvent.UnregisterListener(this);
        keyTakenEvent.UnregisterListener(this);
    }
    public void OnEventRaised(GameEvent gameEvent)
    {
        if(gameEvent == unlockEvent) 
            unlock?.Invoke();

        if(gameEvent == keyTakenEvent)
            keyTaken?.Invoke();
    }
}
