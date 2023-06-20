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

    float horizontalInput;
    float verticalInput;
    public float moveSpeed = 20f;
    private void Awake()
    {
        if (instance == null) instance = this;
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    private void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        if (horizontalInput != 0)
        {
            virtualCamera.transform.Translate(horizontalInput * moveSpeed * Time.deltaTime, 0, 0);
        }
        else if(verticalInput != 0)
        {
            virtualCamera.transform.Translate(0, verticalInput * moveSpeed * Time.deltaTime, 0);
        }
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
