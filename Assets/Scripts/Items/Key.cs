using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Key : MonoBehaviour
{
    public float delayTime = 0.5f;
    Collider2D detectPlayer;

    [SerializeField] GameEvent keyTakenEvent;
    private void Awake()
    {
        detectPlayer = GetComponent<Collider2D>();
        detectPlayer.enabled = false;
        StartCoroutine(EnableCollisionWithPlayer());
    }

    IEnumerator EnableCollisionWithPlayer()
    {
        yield return new WaitForSeconds(delayTime);
        detectPlayer.enabled = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerInteraction>().hasKey = true;
            keyTakenEvent.Raise();
            DestroyKey();
        }
    }

    public void DestroyKey()
    {
        Destroy(this.gameObject);
    }
}
