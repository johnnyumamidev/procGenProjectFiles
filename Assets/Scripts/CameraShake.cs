using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Diagnostics.Tracing;
using UnityEngine.Events;

public class CameraShake : MonoBehaviour
{
    private CinemachineVirtualCamera cinemachineCamera;
    CinemachineBasicMultiChannelPerlin cinemachinePerlin;
    private float shakeTimer;
    public float shakeIntensity;
    public float shakeTime;
    void Awake()
    {
        cinemachineCamera = GetComponent<CinemachineVirtualCamera>();
        cinemachinePerlin = cinemachineCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        EventManager.instance.AddListener("camera_shake", ShakeEvent());
    }

    private UnityAction ShakeEvent()
    {
        UnityAction action = () =>
        {
            ShakeCamera(shakeIntensity, shakeTime);
        };
        return action;
    }

    public void ShakeCamera(float intensity, float time)
    {
        Debug.Log("camera shake");
        
        cinemachinePerlin.m_AmplitudeGain = intensity;
        shakeTimer = time;
    }

    void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0)
            {
                cinemachinePerlin.m_AmplitudeGain = 0f;
            }
        }
    }
}
