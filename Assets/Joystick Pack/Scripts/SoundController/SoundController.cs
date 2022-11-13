using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public static SoundController instance { get; private set; }

    [SerializeField]
    private AudioClip UZI;
    [SerializeField]
    private AudioClip aka47;
    [SerializeField]
    private AudioClip bazooka;
    [SerializeField]
    private AudioClip tazer;
    [SerializeField]
    private AudioClip bazookaExplosion;
    [SerializeField]
    private AudioClip tazerLighting;
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

    public void PlaySoundWeapon(Weapons weapon)
    {
        switch (weapon)
        {
            case Weapons.AKA47:
                PlayAudioClip(aka47);
                break;
            case Weapons.UZI:
                PlayAudioClip(UZI);
                break;
            case Weapons.BAZOOKA:
                PlayAudioClip(bazooka);
                break;
            case Weapons.TAZER:
                PlayAudioClip(tazer);
                break;
        }
    }

    public void PlaySoundBazookaExplosion()
    {
        PlayAudioClip(bazookaExplosion);
    }
    public void PlaySoundTazerLighting()
    {
        PlayAudioClip(tazerLighting);
    }
    public void playSound1()
    {
        PlayAudioClip(enemyDeath);
    }
}
