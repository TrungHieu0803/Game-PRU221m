using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public static SoundController instance { get; private set; }

    public AudioClip shootingSound;
    public AudioClip enemyDeath;

    private AudioSource audioSource;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }

        audioSource = GetComponent<AudioSource>();
    }

    private void PlayAudioClip(AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
    }

    public void playSound()
    {
        PlayAudioClip(shootingSound);
    }
    public void playSound1()
    {
        PlayAudioClip(enemyDeath);
    }
}
