using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    public GameObject keyPrefab;
    public int keyCount = 1;
    public GameObject openEffect;
    AudioSource audioSource;
    public GameObject interactableObject => this.gameObject;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public bool DropItem()
    {
        throw new System.NotImplementedException();
    }

    public bool Interact(PlayerInteraction interactor)
    {
        if(keyCount > 0)
        {
            audioSource.Play();
            openEffect.SetActive(true);
            Instantiate(keyPrefab, transform.position, Quaternion.identity);
            keyCount--;
        }
        gameObject.layer = 0;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        return false;
    }

}
