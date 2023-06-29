using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellLayout : MonoBehaviour
{
    CellWalls cellWalls;
    bool setLayout = false;
    [SerializeField] GameObject elevatorEnter;
    [SerializeField] GameObject elevatorExit;
    [SerializeField] GameObject lockedDoorRoom;
    public List<GameObject> arenas = new List<GameObject>();
    public GameObject chosenArena;
    private void Awake()
    {
        cellWalls = GetComponent<CellWalls>();
        int random = Random.Range(0, arenas.Count);
        chosenArena = arenas[random];
        Debug.Log(gameObject.name + "random index: " + random);
    }

    private void Update()
    {
        if (!setLayout)
        {
            SetRoomLayout();
            setLayout = true;
        }        
    }
    private void SetRoomLayout()
    {
        if (gameObject.tag == "Exit") elevatorExit.SetActive(true);
        else if (gameObject.tag == "LockedRoom") lockedDoorRoom.SetActive(true);
        else if (gameObject.tag == "Start") elevatorEnter.SetActive(true);
        else
        {
            chosenArena.SetActive(true);
        }
    }
}
