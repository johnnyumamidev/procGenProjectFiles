using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName ="NPC/Data")]
public class NPCData : ScriptableObject
{
    public string NPCType;

    public void Action()
    {
        EventManager.instance.TriggerEvent(NPCType + "_event");
    }
}
