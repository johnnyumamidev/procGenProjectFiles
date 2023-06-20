using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WallsManager : MonoBehaviour, IEventListener
{
    public List<GameObject> walls = new List<GameObject>(); // 0123 => NESW
    public List<GameObject> activeWalls = new List<GameObject>();
    public Vector2 direction;
    public Vector2 lastDirection;
    public int _directionindex;
    public int _lastDirectionIndex;
    public bool wallsDisabled = false;
    public int roomIndex = 0;
    public bool isTreasureRoom = false;

    [SerializeField] GameEvent walkCompleteEvent;
    [SerializeField] UnityEvent response;
    [SerializeField] GameEvent wallsDisabledCompleteEvent;
    bool levelComplete = false;
    private void OnEnable()
    {
        walkCompleteEvent.RegisterListener(this);
        wallsDisabled = false;
        foreach(GameObject wall in walls)
        {
            wall.SetActive(true);
            activeWalls.Add(wall);
        }
    }
    private void OnDisable()
    {
        walkCompleteEvent.UnregisterListener(this);   
    }

    public void SetLevelCompleteTrue()
    {
        levelComplete = true;
    }
    void Update()
    {
        if (!levelComplete) return;
        if (isTreasureRoom)
        {
            DisableWallTowardsPreviousRoom();
        }

        if (!wallsDisabled)
        {
            DisableWallTowardsNextRoom(direction);
            if (roomIndex > 0)
            {
                DisableWallTowardsPreviousRoom();
            }

            wallsDisabled = true;
            wallsDisabledCompleteEvent.Raise();
        }
    }

    public void DisableWallToTreasureRoom(Vector2 direction)
    {
        Debug.Log(gameObject.name + " creating path to treasure room " + direction);
        DisableWallTowardsNextRoom(direction);
    }

    public void DisableWallTowardsNextRoom(Vector2 _direction)
    {
        int directionIndex = 0;
        if (_direction == Vector2.up) directionIndex = 0;
        if (_direction == Vector2.right) directionIndex = 1;
        if (_direction == Vector2.down) directionIndex = 2;
        if (_direction == Vector2.left) directionIndex = 3;
        walls[directionIndex].SetActive(false);
        activeWalls.Remove(walls[directionIndex]);
        _directionindex = directionIndex;
    }

    public void DisableWallTowardsPreviousRoom()
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

    public void OnEventRaised(GameEvent gameEvent)
    {
        response?.Invoke();
    }
}
