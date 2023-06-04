using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomRotation : MonoBehaviour
{
    void Update()
    {
        SetRotationToZero();
    }

    private void SetRotationToZero()
    {
        if (transform.rotation == Quaternion.Euler(Vector3.zero)) return;
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
