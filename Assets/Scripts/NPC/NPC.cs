using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public NPCData npcData;
    public void NPCAction()
    {
        npcData.Action();
    }
}
