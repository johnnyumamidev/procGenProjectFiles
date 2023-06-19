using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Lock : MonoBehaviour, IInteractable
{
    [SerializeField] GameEvent unlockEvent;

    Door door;
    public GameObject openEffect;

    public GameObject interactableObject => throw new System.NotImplementedException();

    private void Awake()
    {
        door = GetComponentInParent<Door>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Key key = collision.GetComponent<Key>();
        if (key != null)
        {
        }
    }

    public bool Interact(PlayerInteraction interactor)
    {
        if (interactor.hasKey)
        {
            unlockEvent.Raise();
            Destroy(this.gameObject);
            Instantiate(openEffect, transform.position, Quaternion.identity);
            AudioManager.instance.DoorUnlockSFX();
            door.UnlockDoor();
        }
        else
        {
            Debug.Log("need key to open this lock");
        }
        return false;
    }

    public bool DropItem()
    {
        throw new System.NotImplementedException();
    }
}
