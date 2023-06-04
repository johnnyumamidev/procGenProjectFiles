using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Key : Item
{
    public void DestroyKey()
    {
        Debug.Log("key used");
        Destroy(this.gameObject);
    }
}
