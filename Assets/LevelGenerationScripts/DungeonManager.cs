using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    public Zone currentZone;
    public enum Zone
    {
        Starting, Underworld, Sewers, Castle, ThroneRoom
    }
    public int currentFloor = 1;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
