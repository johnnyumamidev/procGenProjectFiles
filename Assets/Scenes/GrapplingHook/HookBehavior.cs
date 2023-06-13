using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookBehavior : MonoBehaviour
{
    public LayerMask groundLayer;
    Vector2 point;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            Debug.Log("attach hook onto " + collision.gameObject.name);
            Rigidbody2D hook = GetComponent<Rigidbody2D>();
            hook.velocity = Vector2.zero;
            hook.isKinematic = true;
            point = collision.GetContact(0).point;
            transform.position = point;
            hook.freezeRotation = true;
        }
    }
}
