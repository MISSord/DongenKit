using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioServer : MonoBehaviour
{
    public static AudioServer Instance = null; //Singleton

    [Header("AudioSource")] //List of sounds
    public AudioSource backgroundMusic;
    public AudioSource ClipMusic;
    bool isMusicPlay; //for check music status


    public void Init( AudioServer audio)
    {
        if (Instance == null && audio != null)
        {
            Instance = audio;
        }
        else
        {
            Instance = new AudioServer();
        }
        backgroundMusic = GetComponents<AudioSource>()[0];
        ClipMusic = GetComponents<AudioSource>()[1];
    }

    public void PlayMusic(AudioClip music)
    {
        if (ClipMusic.isPlaying)
        {
            ClipMusic.Stop();
        }
        ClipMusic.clip = music;
        ClipMusic.loop = false;
        ClipMusic.Play();
    }

    public void PlayBGround(AudioClip Bg)
    {
        if (backgroundMusic.isPlaying)
        {
            backgroundMusic.Stop();
        }
        backgroundMusic.clip = Bg;
        backgroundMusic.loop = true;
        backgroundMusic.Play();
    }
}

