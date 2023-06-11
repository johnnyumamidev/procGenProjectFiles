using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;

public class CameraPositioner : MonoBehaviour
{
    CinemachineVirtualCamera virtualCamera;
    public static CameraPositioner instance;
    public Transform roomToFollow;
    private void Awake()
    {
        if (instance == null) instance = this;
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    public void SetCamera()
    {
        Debug.Log("setting camera to current room");
        virtualCamera.Follow = roomToFollow;
    }
}
