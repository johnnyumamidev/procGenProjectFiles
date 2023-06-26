using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellLayout : MonoBehaviour
{
    CellWalls cellWalls;
    bool setLayout = false;
    [SerializeField] GameObject elevatorEnter;
    [SerializeField] GameObject elevatorExit;

    private void Awake()
    {
        cellWalls = GetComponent<CellWalls>();
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
        else if(gameObject.tag == "Start") elevatorEnter.SetActive(true);
    }
}
