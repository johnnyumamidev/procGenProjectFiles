using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class BoltBehavior : MonoBehaviour, ICollectable
{
    Rigidbody2D boltRigidbody;
    public Collider2D boltCollider;
    public LayerMask playerLayer;
    public LayerMask groundLayer;
    public float boltSpeed = 10f;
    public Vector2 boltDirection;

    [SerializeField] GameEvent collectBolt;
    private void Awake()
    {
        boltRigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        boltRigidbody.AddForce(boltDirection * boltSpeed);
    }

    private void Update()
    {
        if (boltCollider.IsTouchingLayers(playerLayer))
        {
            Debug.Log("bolt collected");
            Collect();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag != "Player")
        {
            Rigidbody2D bolt = GetComponent<Rigidbody2D>();
            bolt.freezeRotation = true;
            bolt.velocity = Vector2.zero;
            bolt.isKinematic = true;
            Vector2 point;
            point = collision.GetContact(0).point;
            transform.position = point;
        }
    }
    public void Collect()
    {
        collectBolt.Raise();
        Destroy(gameObject); 
    }
}
