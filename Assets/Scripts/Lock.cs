using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Lock : MonoBehaviour
{
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
            EventManager.instance.TriggerEvent("unlock");
            AudioManager.instance.DoorUnlockSFX();
            Instantiate(openEffect, transform.position, Quaternion.identity);
            door.UnlockDoor();
            key.DestroyKey();
            Destroy(this.gameObject);
        }
    }

}
