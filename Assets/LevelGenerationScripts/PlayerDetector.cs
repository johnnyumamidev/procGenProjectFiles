using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    [SerializeField] GameEvent gameEvent;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CameraPositioner.instance.roomToFollow = gameObject.transform;
            gameEvent.Raise();
        }
    }
}
