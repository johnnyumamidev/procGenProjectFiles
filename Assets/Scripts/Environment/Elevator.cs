using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Events;

public class Elevator : MonoBehaviour
{
    [SerializeField] Rigidbody2D elevatorRb;
    bool elevatorActive = false;
    public float speed = 2;
    
    private void Update()
    {
        Vector2 velocity = Vector2.zero;
        if (elevatorActive)
            velocity = Vector2.up * speed * Time.deltaTime;

        elevatorRb.velocity = velocity;
    }
    public void ActivateElevator()
    {
        elevatorActive = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Stop"))
        {
            elevatorActive = false;
            Debug.Log("stop");
        }
    }
}
