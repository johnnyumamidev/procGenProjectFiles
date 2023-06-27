using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    bool doorLocked = true;
    [SerializeField] GameEvent doorOpenedEvent;

    public GameObject interactableObject => this.gameObject;

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
            Debug.Log("door unlocked, proceed to next room");
        }

        return false;
    }

    public void UnlockDoor()
    {
        doorLocked = false;
    }
}
