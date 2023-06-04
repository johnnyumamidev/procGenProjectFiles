using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationAudioHandler : MonoBehaviour
{
    public AudioSource audioSource;

    private void PlayAudio(AudioClip audio)
    {
        audioSource.clip = audio;
        audioSource.Play();
    }
}
