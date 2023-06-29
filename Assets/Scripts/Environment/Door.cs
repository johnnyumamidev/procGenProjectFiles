using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    SpriteRenderer spriteRenderer;
    bool doorLocked = true;
    [SerializeField] GameEvent doorOpenedEvent;
    public GameObject doorLock;
    public Sprite openDoorSprite;

    public GameObject interactableObject => this.gameObject;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public bool DropItem()
    {
        throw new System.NotImplementedException();
    }

    public bool Interact(PlayerInteraction interactor)
    {
        if (doorLocked) { Debug.Log("door locked"); }
        else
        {
            doorOpenedEvent.Raise();
            spriteRenderer.sprite = openDoorSprite;
            Debug.Log("door unlocked, proceed to next room");
        }

        return false;
    }

    public void UnlockDoor()
    {
        doorLocked = false;
        doorLock.SetActive(false);
    }
}
