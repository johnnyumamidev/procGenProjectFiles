using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour, ICollectable
{
    bool isCollectable = false;
    public float delayTime = 0.5f;
    private void Start()
    {
        StartCoroutine(DelayAfterSpawning());
    }

    private IEnumerator DelayAfterSpawning()
    {
        yield return new WaitForSeconds(delayTime);
        isCollectable = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && isCollectable)
        {
            Collect();
        }
    }

    public void Collect()
    {
        EventManager.instance.TriggerEvent("increase_currency");
        Destroy(transform.parent.gameObject);
    }
}
