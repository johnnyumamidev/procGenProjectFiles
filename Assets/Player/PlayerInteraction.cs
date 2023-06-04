using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static UnityEditor.Progress;

public class PlayerInteraction : MonoBehaviour
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
    private readonly Collider2D[] colliders = new Collider2D[2];
    [SerializeField] private int numberFound;

    public bool currentlyHoldingItem = false;
    public IInteractable currentlyHeldItem;

    private void Awake()
    {
        EventManager.instance.AddListener("unlock", DoorUnlocked());
        interactionTimer = interactionCooldown;
        playerInput = GetComponent<PlayerInput>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    private UnityAction DoorUnlocked()
    {
        UnityAction action = () => 
        {
            Debug.Log("door unlocked, dropping held item");
            currentlyHoldingItem = false;
        };
        return action;
    }

    void Update()
    {
        pointer.SetActive(false);
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
            var interactable = colliders[0].GetComponent<IInteractable>();
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
        }

        if (currentlyHoldingItem && playerInput.performInteract != 0 && interactionReady)
        {
            audioSource.clip = pickUpSFX;
            audioSource.Play();
            DropCurrentlyHeldItem();
            interactionReady = false;
        }

        if (currentlyHoldingItem && playerHealth.playerHurtState)
        {
            Debug.Log("hurt !! dropping " + currentlyHeldItem.interactableObject.name);
            DropCurrentlyHeldItem();
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(interactionHitbox.position, interactionRadius);
    }
}
