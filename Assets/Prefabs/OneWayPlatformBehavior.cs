using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayPlatformBehavior : MonoBehaviour
{
    Collider2D platformCollider;
    public float resetColliderTime = 0.5f;
    private void Awake()
    {
        platformCollider = GetComponent<Collider2D>();
    }

    public void PlayerFallThrough()
    {
        platformCollider.enabled = false;
        StartCoroutine(ResetCollider());
    }

    private IEnumerator ResetCollider()
    {
        while (!platformCollider.enabled)
        {
            yield return new WaitForSeconds(resetColliderTime);
            platformCollider.enabled = true;
        }
    }
}
