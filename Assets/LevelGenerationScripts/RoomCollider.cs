using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCollider : MonoBehaviour
{
    ObstacleSpawner obstacleSpawner;
    Collider2D roomCollider;
    Rigidbody2D roomRigidbody;
    private void Awake()
    {
        obstacleSpawner = GetComponent<ObstacleSpawner>();
    }
    void Start()
    {
        roomCollider = gameObject.AddComponent<BoxCollider2D>();
        roomCollider.isTrigger = false;
        roomRigidbody = gameObject.AddComponent<Rigidbody2D>();
        roomRigidbody.gravityScale = 0;

        StartCoroutine(DisableCollider());
    }

    private IEnumerator DisableCollider()
    {
        yield return new WaitForSeconds(2);
        roomCollider.enabled = false;
        obstacleSpawner.SpawnTiles();
        StopCoroutine(DisableCollider());
    }
}
