using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallsManager : MonoBehaviour
{
    public List<GameObject> walls = new List<GameObject>(); // 0123 => NESW
    public List<GameObject> activeWalls = new List<GameObject>();
    public Vector2 direction;
    public Vector2 lastDirection;
    public int _directionindex;
    public int _lastDirectionIndex;
    public bool wallsDisabled = false;
    public int roomIndex = 0;
    private void OnEnable()
    {
        wallsDisabled = false;
        foreach(GameObject wall in walls)
        {
            wall.SetActive(true);
            activeWalls.Add(wall);
        }
    }
    void Update()
    {
        if (!wallsDisabled)
        {
            int directionIndex = 0;
            if (direction == Vector2.up) directionIndex = 0;
            if (direction == Vector2.right) directionIndex = 1;
            if (direction == Vector2.down) directionIndex = 2;
            if (direction == Vector2.left) directionIndex = 3;
            walls[directionIndex].SetActive(false);
            activeWalls.Remove(walls[directionIndex]);
            _directionindex = directionIndex;
            if (roomIndex > 0)
            {
                int lastDirectionIndex = 0;
                if (lastDirection == Vector2.up) lastDirectionIndex = 2;
                if (lastDirection == Vector2.right) lastDirectionIndex = 3;
                if (lastDirection == Vector2.down) lastDirectionIndex = 0;
                if (lastDirection == Vector2.left) lastDirectionIndex = 1;
                walls[lastDirectionIndex].SetActive(false);
                activeWalls.Remove(walls[lastDirectionIndex]);
                _lastDirectionIndex = lastDirectionIndex;
            }

            wallsDisabled = true;
        }
    }
}
