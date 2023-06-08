using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour, ICollectable
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Collect();
        }
    }

    public void Collect()
    {
        Debug.Log("gem collected");
        Destroy(transform.parent.gameObject);
    }
}
