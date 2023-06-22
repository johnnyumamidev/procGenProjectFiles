using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    [SerializeField] GameEvent playerEntersEvent;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (CameraPositioner.instance == null) return;
            CameraPositioner.instance.roomToFollow = gameObject.transform;
            CameraPositioner.instance.playerTransform = collision.transform;
            playerEntersEvent.Raise();
        }
    }
}
