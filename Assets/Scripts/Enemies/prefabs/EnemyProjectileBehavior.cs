using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class EnemyProjectileBehavior : MonoBehaviour
{
    public LayerMask groundLayer;
    [SerializeField] GameEvent playerDamageEvent;
   
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
