using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] GameEvent NPCEvent;

    public void NPCAction()
    {
        NPCEvent.Raise();
    }
}
