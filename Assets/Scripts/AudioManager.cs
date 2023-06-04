using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource audioSource;
    public AudioClip openDoorSFX;
    public AudioClip unlockSFX;
    public AudioClip playerDamageSFX;
    private void Awake()
    {
        instance = this;
    }
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
}
