using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class EnemyProjectileBehavior : MonoBehaviour
{
    public LayerMask groundLayer;
    [SerializeField] GameEvent playerDamageEvent;
    Rigidbody2D bulletRigidbody;
    float bulletSpeed = 25f;
    Vector2 bulletVelocity;
    private void Start()
    {
        bulletRigidbody = GetComponent<Rigidbody2D>();
    }

    public void SetVelocity(float _centerVectorAngle, float _angleStep, bool facingRight)
    {
        if (!facingRight)
        {
            bulletSpeed *= -1;
        }
        bulletVelocity = new Vector2(Mathf.Cos(_centerVectorAngle + Mathf.PI / 2 + _angleStep), Mathf.Sin(_centerVectorAngle + Mathf.PI / 2 + _angleStep)) * bulletSpeed * Time.fixedDeltaTime;
    }
    private void FixedUpdate()
    {
        bulletRigidbody.velocity = bulletVelocity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            Debug.Log("hit wall, destroying self");
            Destroy(gameObject);
        }

        if (collision.CompareTag("Player"))
        {
            Debug.Log("hit player");
            playerDamageEvent.Raise();
            Destroy(gameObject);
        }
    }
}
