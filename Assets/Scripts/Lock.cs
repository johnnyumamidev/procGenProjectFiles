using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Lock : MonoBehaviour
{
    [SerializeField] GameEvent unlockEvent;

    Door door;
    public GameObject openEffect;
    private void Awake()
    {
        door = GetComponentInParent<Door>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Key key = collision.GetComponent<Key>();
        if (key != null)
        {
            unlockEvent.Raise();
            AudioManager.instance.DoorUnlockSFX();
            Instantiate(openEffect, transform.position, Quaternion.identity);
            door.UnlockDoor();
            key.DestroyKey();
            Destroy(this.gameObject);
        }
    }

}
