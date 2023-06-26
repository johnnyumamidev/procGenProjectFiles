using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellWalls : MonoBehaviour
{
    public GameObject north, east, south, west;
    public Vector2 directionTowardsNextRoom;
    public Vector2 previousRoomDirection;
    bool wallsDisabled = false;

    private void Update()
    {
        if (!wallsDisabled)
        {
            DisableWalls();
            wallsDisabled = true;
        }
    }

    private void DisableWalls()
    {
        if(directionTowardsNextRoom == Vector2.up) north.SetActive(false);
        else if(directionTowardsNextRoom == Vector2.down) south.SetActive(false);
        else if(directionTowardsNextRoom == Vector2.left) west.SetActive(false);
        else if(directionTowardsNextRoom == Vector2.right) east.SetActive(false);  

        if(previousRoomDirection == Vector2.up) { south.SetActive(false); }
        else if(previousRoomDirection == Vector2.down) { north.SetActive(false); }
        else if(previousRoomDirection == Vector2.left) { east.SetActive(false); }
        else if(previousRoomDirection == Vector2.right) { west.SetActive(false); }
    }
}
