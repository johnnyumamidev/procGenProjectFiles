using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour, IInteractable
{
    public GameObject interactableObject => this.gameObject;
    public GameObject gear;

    [SerializeField] GameEvent leverPulled;
    public bool DropItem()
    {
        throw new System.NotImplementedException();
    }

    public bool Interact(PlayerInteraction interactor)
    {
        Debug.Log("pull lever");
        gear.transform.rotation = Quaternion.Euler(0, 0, 135);
        leverPulled.Raise();
        return false;
    }
}
