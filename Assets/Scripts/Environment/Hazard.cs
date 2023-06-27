using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    [SerializeField] GameEvent playerDamage;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameStateManager.instance.currentState == "Game Over") return;
        if (collision.gameObject.CompareTag("Player"))
        {
            playerDamage.Raise();
        }
    }
}
