using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] DungeonManager dungeonManager;
    [SerializeField] int numberOfElevatorRooms = 2;
    [SerializeField] int width;
    [SerializeField] int height;
    [HideInInspector] public int gridWidth;
    [HideInInspector] public int gridHeight;
    public int cellWidth;
    public int cellHeight;
    public Transform startGridPosition;
    Vector2 point;
    public List<Vector2> points = new List<Vector2>();
   
    
}
