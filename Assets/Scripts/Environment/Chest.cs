using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Chest : MonoBehaviour, IInteractable
{
    SpriteRenderer spriteRenderer;
    public Sprite openChestSprite;

    public GameObject itemPrefab;
    public int remainingItems = 1;
    public GameObject openEffect;
    public float itemLaunchForce = 1.5f;
    Vector2 spawnDirection;
    public float timeBetweenItemSpawn = 0.02f;

    AudioSource audioSource;
    public GameObject interactableObject => this.gameObject;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }
    public bool DropItem()
    {
        throw new System.NotImplementedException();
    }

    public bool Interact(PlayerInteraction interactor)
    {
        audioSource.Play();
        openEffect.SetActive(true);
        spriteRenderer.sprite = openChestSprite;

        StartCoroutine(SpawnItem());

        gameObject.layer = 0;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        return false;
    }

    private IEnumerator SpawnItem()
    {
        while (remainingItems > 0)
        {
            GameObject item = Instantiate(itemPrefab, transform.position, Quaternion.identity);
            Rigidbody2D itemRigidbody = item.GetComponent<Rigidbody2D>();

            float randomXDirection = Random.Range(-0.25f, 0.25f);
            spawnDirection = new Vector2(randomXDirection, 1);
            itemRigidbody.AddRelativeForce(spawnDirection * itemLaunchForce, ForceMode2D.Impulse);
            remainingItems--;
            yield return new WaitForSeconds(timeBetweenItemSpawn);
        }
    }
}
