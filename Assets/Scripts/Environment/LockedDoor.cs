using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoor : MonoBehaviour
{
    public float speed = 50f;
    Rigidbody2D doorRb;
    bool doorActive = false;
    private void Awake()
    {
        doorRb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        Vector2 velocity = Vector2.zero;
        if (doorActive)
            velocity = Vector2.up * speed * Time.deltaTime;

        doorRb.velocity = velocity;
    }
    public void LiftDoor()
    {
        doorActive = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Stop"))
        {
            doorActive = false;
            Debug.Log("stop");
        }
    }
}
