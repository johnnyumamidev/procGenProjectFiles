using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour
{
    public GameObject nextRoomDirection;
    public List<Transform> directions = new List<Transform>();

    public void SetPointer(Vector2 direction)
    {
        if(direction == Vector2.up)
        {
            nextRoomDirection.transform.position = directions[0].position;
            nextRoomDirection.transform.rotation = directions[0].rotation;
        }
        if(direction == Vector2.right)
        {
            nextRoomDirection.transform.position = directions[1].position;
            nextRoomDirection.transform.rotation = directions[1].rotation;

        }
         if(direction == Vector2.down)
        {
            nextRoomDirection.transform.position = directions[2].position;
            nextRoomDirection.transform.rotation = directions[2].rotation;

        }
         if(direction == Vector2.left)
        {
            nextRoomDirection.transform.position = directions[3].position;
            nextRoomDirection.transform.rotation = directions[3].rotation;

        }

    }
}
