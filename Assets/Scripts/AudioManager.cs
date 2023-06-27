using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AudioManager : MonoBehaviour, IEventListener
{
    [SerializeField] GameEvent playerDamageEvent;
    [SerializeField] UnityEvent playerDamage;
    [SerializeField] GameEvent unlockEvent;
    [SerializeField] UnityEvent unlock;
    [SerializeField] GameEvent doorOpenedEvent;
    [SerializeField] UnityEvent doorOpened;
    public AudioSource audioSource;
    public AudioClip openDoorSFX;
    public AudioClip unlockSFX;
    public AudioClip playerDamageSFX;
    public void DoorUnlockSFX()
    {
        audioSource.clip = unlockSFX;
        audioSource.Play();
    }

    public void OpenDoor()
    {
        audioSource.clip = openDoorSFX;
        audioSource.Play();
    }

    public void PlayerDamage()
    {
        audioSource.clip = playerDamageSFX;
        audioSource.Play();
    }
    private void OnEnable()
    {
        playerDamageEvent.RegisterListener(this);
        unlockEvent.RegisterListener(this);
        doorOpenedEvent.RegisterListener(this);
    }
    private void OnDisable()
    {
        playerDamageEvent.UnregisterListener(this);
        unlockEvent.UnregisterListener(this);
        doorOpenedEvent.UnregisterListener(this);
    }
    public void OnEventRaised(GameEvent gameEvent)
    {
        if (gameEvent == unlockEvent) unlock?.Invoke();
        if(gameEvent == playerDamageEvent) playerDamage?.Invoke();
        if(gameEvent == doorOpenedEvent) doorOpened?.Invoke();
    }
}
