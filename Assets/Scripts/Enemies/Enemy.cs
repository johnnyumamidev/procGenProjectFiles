using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    protected static int currentHealth;

    protected abstract void Movement();
    protected abstract void DetectPlayer();

    protected abstract void TakeDamage();

}
