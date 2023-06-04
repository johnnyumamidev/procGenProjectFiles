using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CameraPositioner.instance.roomToFollow = gameObject.transform;
            EventManager.instance.TriggerEvent("set_camera");
        }
    }
}
