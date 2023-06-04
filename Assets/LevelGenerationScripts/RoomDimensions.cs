using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomDimensions : MonoBehaviour
{
    public GameObject roomBoundaries;
    public Vector3 roomDimensions;
    void Start()
    {
        roomBoundaries.transform.localScale = roomDimensions;
    }
}
