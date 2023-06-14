using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;

public class CameraPositioner : MonoBehaviour, IEventListener
{
    CinemachineVirtualCamera virtualCamera;
    public static CameraPositioner instance;
    public Transform roomToFollow;
    public Transform playerTransform;
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

    public void FollowPlayer()
    {
        virtualCamera.Follow = playerTransform;
    }

    [SerializeField] GameEvent playerEntersRoomEvent;
    [SerializeField] UnityEvent playerEntersRoom;

    private void OnEnable()
    {
        playerEntersRoomEvent.RegisterListener(this);
    }
    private void OnDisable()
    {
        playerEntersRoomEvent.UnregisterListener(this);
    }
    public void OnEventRaised(GameEvent gameEvent)
    {
        playerEntersRoom.Invoke();
    }
}
