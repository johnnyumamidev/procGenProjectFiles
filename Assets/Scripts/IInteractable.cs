using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public GameObject interactableObject { get; }

    public bool Interact(PlayerInteraction interactor);

    public bool DropItem();
}
