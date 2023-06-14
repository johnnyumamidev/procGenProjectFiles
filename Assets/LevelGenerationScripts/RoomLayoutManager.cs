using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomLayoutManager : MonoBehaviour
{
    WallsManager wallsManager;
    public GameObject startRoomPrefab;
    GameObject startRoom;
    public GameObject exitRoomPrefab;
    GameObject exitRoom;
    bool roomsSet = false;
    bool startingRoomPlaced = false;
    bool exitRoomPlaced = false;
    public List<GameObject> treasureRoom = new List<GameObject>();
    public List<GameObject> shopRoom = new List<GameObject>();

    public List<GameObject> NE = new List<GameObject>();
    public List<GameObject> NW = new List<GameObject>();
    public List<GameObject> NS = new List<GameObject>();
    public List<GameObject> SW = new List<GameObject>();
    public List<GameObject> SE = new List<GameObject>();
    public List<GameObject> EW = new List<GameObject>();

    public List<GameObject> roomLayouts;
    void Awake()
    {
        startRoom = Instantiate(startRoomPrefab, transform);
        exitRoom = Instantiate(exitRoomPrefab, transform);
        startRoom.SetActive(false);
        exitRoom.SetActive(false);
        wallsManager = GetComponent<WallsManager>();
    }
    private void SetRoomLayout()
    {
        int randomIndex = Random.Range(0, roomLayouts.Count - 1);
        if (roomLayouts.Count == 1) randomIndex = 0;
        roomLayouts[randomIndex].SetActive(true);
    }
    private void Update()
    {
        if (!startingRoomPlaced && gameObject.CompareTag("Start"))
        {
            foreach (GameObject room in roomLayouts)
            {
                room.SetActive(false);
            }
            roomLayouts = new List<GameObject> { startRoom };
            roomLayouts[0].SetActive(true);
            roomsSet = true;
            startingRoomPlaced = true;
        }
        else if(!exitRoomPlaced && gameObject.CompareTag("Exit"))
        {
            foreach(GameObject room in roomLayouts)
            {
                room.SetActive(false);
            }
            roomLayouts = new List<GameObject> { exitRoom };
            roomLayouts[0].SetActive(true);
            roomsSet = true;
            exitRoomPlaced = true;
        }

        if (roomsSet) return;

        if (wallsManager.walls[0].activeSelf && wallsManager.walls[1].activeSelf) roomLayouts = SW;
        else if (wallsManager.walls[0].activeSelf && wallsManager.walls[3].activeSelf) roomLayouts = SE;
        else if (wallsManager.walls[1].activeSelf && wallsManager.walls[2].activeSelf) roomLayouts = NW;
        else if (wallsManager.walls[2].activeSelf && wallsManager.walls[3].activeSelf) roomLayouts = NE;
        else if (wallsManager.walls[0].activeSelf && wallsManager.walls[2].activeSelf) roomLayouts = EW;
        else if (wallsManager.walls[1].activeSelf && wallsManager.walls[3].activeSelf) roomLayouts = NS;

        if (gameObject.CompareTag("Filler"))
        {
            foreach (GameObject room in roomLayouts)
            {
                room.SetActive(false);
            }
        }
        if (roomLayouts.Count > 0 && !roomsSet)
        {
            SetRoomLayout();
            roomsSet = true;
        }
    }

    private void OnDisable()
    {
        foreach (GameObject room in roomLayouts)
        {
            room.SetActive(false);
        }
        roomsSet = false;
    }
}
