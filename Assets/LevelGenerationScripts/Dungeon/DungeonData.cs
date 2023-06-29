using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Dungeon/Data")]
public class DungeonData : ScriptableObject
{
    public List<GameObject> arenas= new List<GameObject>();
    public int startingFloorCount = 2;
}
