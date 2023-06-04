using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileColliderScaler : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    BoxCollider2D boxCollider;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }
    void Update()
    {
        boxCollider.size = spriteRenderer.size;
    }
}
