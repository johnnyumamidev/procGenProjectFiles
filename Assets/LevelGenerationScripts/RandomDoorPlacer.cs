using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RandomDoorPlacer : MonoBehaviour
{
    public GameObject doorParent;
    public GameObject wallsParent;
    public List<Transform> doors = new List<Transform>();
    public List<Transform> walls = new List<Transform>();
    RoomDimensions roomDimensions;
    void Start()
    {
        roomDimensions = GetComponent<RoomDimensions>();
        Transform[] _doors = doorParent.GetComponentsInChildren<Transform>();
        foreach(Transform door in _doors)
        {
            doors.Add(door);
        }
        Transform[] _walls = wallsParent.GetComponentsInChildren<Transform>();
        foreach(Transform wall in _walls)
        {
            walls.Add(wall);
        }
        doors.Remove(doors[0]);
        walls.Remove(walls[0]);
        RandomlyPlaceDoors();
    }
    private void RandomlyPlaceDoors()
    {
        for (int i = 0; i < doors.Count; i++)
        {
            doors[i].gameObject.SetActive(false);
            float randomPercent = Random.Range(-0.6f, 0.6f);
            if (i == 0 || i == 1) // North or South Walls
            {
                float xPosition = (randomPercent * 2) / 2;
                Vector2 doorPositionsNS = new Vector2(xPosition + walls[i].transform.position.x, walls[i].transform.position.y);
                Debug.Log(randomPercent + " " + gameObject.name + " "+doorPositionsNS);
                doors[i].transform.position = doorPositionsNS;
            }
            if (i == 2 || i == 3) // East or West Walls  
            {
                float yPosition = (randomPercent * 2) / 2;
                Vector2 doorPositionsEW = new Vector2(walls[i].transform.position.x, yPosition + walls[i].transform.position.y);
                Debug.Log(gameObject.name + " "+ doorPositionsEW);
                doors[i].transform.position = doorPositionsEW;
            }
            Debug.Log(gameObject.name + "// door: " + i + " " + doors[i].transform.localPosition);
        }
    }
}
