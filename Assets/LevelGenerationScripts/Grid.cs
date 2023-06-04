using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public int gridWidth;
    public int gridHeight;
    public int cellWidth;
    public int cellHeight;
    public Transform startGridPosition;
    Vector2 point;
    public List<Vector2> points = new List<Vector2>();
    void Awake()
    {
        GenerateGrid(); 
    }

    private void GenerateGrid()
    {
        for (int x = 1; x <= gridWidth; x += cellWidth)
        {
            for (int y = 1; y <= gridHeight; y += cellHeight)
            {
                point = new Vector2(startGridPosition.position.x + (x * cellWidth), startGridPosition.position.y + (y * cellHeight));
                points.Add(point);
            }
        }
    }
}
