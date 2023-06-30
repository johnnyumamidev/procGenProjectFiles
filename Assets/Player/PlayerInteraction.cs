using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

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
    PlayerLocomotion playerLocomotion;
    private readonly Collider2D[] interactableColliders = new Collider2D[2];
    [SerializeField] private int numberOfInteractablesInRange;

    public GameObject itemObject;
    public bool hasKey = false;

    [Header("Starting Weapons")]
    public GameObject startingMeleeWeapon;
    public GameObject startingRangedWeapon;

    [Header("Equipped Weapons")]
    public GameObject equippedMeleeWeapon;
    public GameObject equippedRangedWeapon;

    [SerializeField] GameEvent weaponPickUpEvent;

    bool npcDetected;
    NPC currentNPC;
    private void Awake()
    {
        GameObject melee = Instantiate(startingMeleeWeapon, transform.position, Quaternion.identity);
        GameObject ranged = Instantiate(startingRangedWeapon, transform.position, Quaternion.identity);
        Weapon meleeWeapon = melee.GetComponent<Weapon>();
        Weapon rangedWeapon = ranged.GetComponent<Weapon>();
        PickUpItem(meleeWeapon, meleeWeapon.interactableObject);
        PickUpItem(rangedWeapon, rangedWeapon.interactableObject);
        
        interactionTimer = interactionCooldown;
        playerInput = GetComponent<PlayerInput>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
    }

    public void HandleInteraction()
    {
        npcDetected = false;
        pointer.SetActive(false);
        if (!playerLocomotion.isGrounded) return;
        GetInteractableObjects();
        CountTimeUntilInteractionReady();
        if (interactionReady && numberOfInteractablesInRange > 0)
        {
            EnablePointer();
            DetectPlayerInteractionInput();
        }
        else
        {
            pointer.SetActive(false);
        }
    }

    private void DetectPlayerInteractionInput()
    {
        if (playerInput.performInteract != 0)
        {
            itemObject = interactableColliders[0].gameObject;
            IInteractable interactable = itemObject.GetComponent<IInteractable>();
            GameObject interactableObj = interactable.interactableObject;
            //if (interactable == null) return;

            PickUpItem(interactable, interactableObj);

            DetectNPC();
        }
    }

    private void PickUpItem(IInteractable interactable, GameObject interactableObj)
    {
        interactionReady = false;
        if (interactableObj.CompareTag("Holdable"))
        {
            audioSource.clip = pickUpSFX;
            audioSource.Play();
            interactableObj.layer = 0;
            Weapon weapon = interactableObj.GetComponent<Weapon>();
            bool isRangedWeapon = weapon.weaponData.isRangedWeapon;
            if (weapon != null)
            {
                weaponPickUpEvent?.Raise();
                interactable.Interact(this);
                if (equippedMeleeWeapon != null && equippedRangedWeapon != null)
                {
                    if (!isRangedWeapon) DropCurrentlyHeldItem(equippedMeleeWeapon);
                    else { DropCurrentlyHeldItem(equippedRangedWeapon); }
                }
                AssignEquippedWeapons(interactableObj, isRangedWeapon);
            }
        }
    }

    private void AssignEquippedWeapons(GameObject weapon, bool ranged)
    {
        Debug.Log("assigning " + weapon.name + " to appropriate slot");
        if (ranged) equippedRangedWeapon = weapon;
        else { equippedMeleeWeapon = weapon; }
    }

    private void DetectNPC()
    {
        if (interactableColliders[0].CompareTag("NPC"))
        {
            npcDetected = true;
            currentNPC = interactableColliders[0].GetComponent<NPC>();
        }
        if (npcDetected)
        {
            if (currentNPC != null) currentNPC.NPCAction();
        }
    }

    private void EnablePointer()
    {
        Vector2 objectPosition = interactableColliders[0].transform.position;
        pointer.SetActive(true);
        pointer.transform.position = objectPosition + pointerOffset;
    }

    private void GetInteractableObjects()
    {
        numberOfInteractablesInRange = Physics2D.OverlapCircleNonAlloc(interactionHitbox.position, interactionRadius, interactableColliders, interactionLayerMask);
    }
    private void CountTimeUntilInteractionReady()
    {
        if (!interactionReady)
        {
            interactionTimer -= Time.deltaTime;
        }
        if (interactionTimer <= 0)
        {
            interactionReady = true;
            interactionTimer = interactionCooldown;
        }
    }

    private void DropCurrentlyHeldItem(GameObject obj)
    {
        audioSource.clip = pickUpSFX;
        audioSource.Play();
        obj.transform.parent = null;
        interactionReady = false;
        obj.layer = 6;
        Weapon weapon = obj.GetComponent<Weapon>();
        if (weapon != null) weapon.DropItem();
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
