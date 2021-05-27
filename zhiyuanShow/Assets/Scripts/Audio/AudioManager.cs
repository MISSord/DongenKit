using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioManager : BaseManager
{
    public static AudioManager Instance; //Singleton

    AudioSource backgroundMusic;
    bool isMusicPlay; //for check music status

    [Header("AudioClips")] //List of sounds
    public AudioClip aiDamage;
    public AudioClip playerDamage;

    public AudioClip pickUpKey;
    public AudioClip pickUpItems;
    public AudioClip pickUpCoin;
    public AudioClip openDoor;
    public AudioClip drinkBottle;

    public AudioClip music;

    //Singleton
    public override void Init()
    {
        if (AudioManager.Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        backgroundMusic = GetComponent<AudioSource>();
        base.Init();

    }

    public void PlayMusic(AudioClip music)
    {
        if (!isMusicPlay) //if the music is not playing
        {
            isMusicPlay = true;
            Play(backgroundMusic, music, true); //Play music
        }
        else
        {
            backgroundMusic.Stop();
            backgroundMusic.clip = music;
            backgroundMusic.Play();
        }
    }

    public void Play(AudioSource audioSource, AudioClip audioClip, bool loop)
    {
        audioSource.clip = audioClip;
        audioSource.loop = loop;
        audioSource.Play();
    }
}

