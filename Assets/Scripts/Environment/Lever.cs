using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Lever : MonoBehaviour, IInteractable
{
    public GameObject interactableObject => this.gameObject;
    public GameObject gear;

    [SerializeField] UnityEvent pullLever;
    public bool DropItem()
    {
        throw new System.NotImplementedException();
    }

    public bool Interact(PlayerInteraction interactor)
    {
        Debug.Log("pull lever");
        gear.transform.rotation = Quaternion.Euler(0, 0, 135);
        pullLever.Invoke();
        return false;
    }
}
