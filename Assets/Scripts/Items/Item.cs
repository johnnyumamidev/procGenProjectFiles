using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour, IInteractable
{
    Rigidbody2D itemRigidbody;
    [SerializeField] BoxCollider2D itemCollider;

    public bool isBeingCarried;
    public Transform playerInteractor;
    public PlayerInteraction playerInteraction;
    PlayerLocomotion playerLocomotion;

    public Vector2 carryOffset;

    public GameObject interactableObject { get => this.gameObject; }

    private void OnEnable()
    {
        itemRigidbody = GetComponent<Rigidbody2D>();
        if(itemCollider == null) itemCollider = GetComponent<BoxCollider2D>();
    }

    public bool DropItem()
    {
        Debug.Log("dropping: " + gameObject.name);
        isBeingCarried = false;
        Vector2 dropPosition = Vector2.zero;
        if (playerInteractor != null)
        {
            dropPosition = playerInteraction.interactionHitbox.position;
        }
        transform.position = dropPosition;
        transform.parent = null;
        playerInteractor = null;
        playerInteraction = null;
        return false;
    }

    public bool Interact(PlayerInteraction interactor)
    {
        Debug.Log("picked up: " + gameObject.name);
        isBeingCarried = !isBeingCarried;
        playerInteraction = interactor;
        playerInteractor = playerInteraction.transform;
        transform.SetParent(playerInteractor);
        return false;
    }


    protected virtual void Update()
    {
        if (itemCollider != null) itemCollider.isTrigger = false;
        itemRigidbody.isKinematic = false;
        Vector2 playerPosition = Vector2.zero;
        if (playerInteractor != null)
        {
            playerPosition = playerInteractor.position;
            Vector3 rotation = new Vector3(0, 0, -90);
            playerLocomotion = playerInteractor.GetComponent<PlayerLocomotion>();
            if (!playerLocomotion.facingRight) { rotation *= -1; }
            transform.rotation = Quaternion.Euler(rotation);
        }
        if (isBeingCarried)
        {
            transform.position = playerPosition + carryOffset;
            itemRigidbody.isKinematic = true;
            itemCollider.isTrigger = true;
        }
    }
}
