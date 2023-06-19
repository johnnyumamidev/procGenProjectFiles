using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Key : MonoBehaviour, ICollectable
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
            Collect();
        }
    }

    public void DestroyKey()
    {
        Destroy(this.gameObject);
    }

    public void Collect()
    {
        keyTakenEvent.Raise();
        DestroyKey();
    }
}
