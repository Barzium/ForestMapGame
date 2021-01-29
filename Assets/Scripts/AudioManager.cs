﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

[System.Serializable]
public class Sound
{
    [SerializeField]
    public AudioClip audioClip;
    [SerializeField]
    public string name;
}

public class AudioManager : MonoBehaviour
{
    public AudioSource[] musicLayers;
    [SerializeField]
    public Sound[] sounds;
    public static AudioManager _instance;
    public static AudioManager Instance { get { return _instance; } }
    private AudioSource audioSource;


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        StartCoroutine(SoundLoop("Crow", 5, 0.5f, 0.1f));
        StartCoroutine(SoundLoop("Cricket", 7, 0.5f, 0.1f));
        StartCoroutine(StartMusic());
    }

    private IEnumerator StartMusic()
    {
        foreach (var item in musicLayers)
        {
            item.Play();
            yield return new WaitForSeconds(1.5f);
        }
    }

    private IEnumerator SoundLoop(string soundName, int PerMin, float volume = 0.5f, float randPitchDepth = 0)
    {
        float baseRate = 60 / PerMin;

        while (true)
        {
            yield return new WaitForSeconds(baseRate + UnityEngine.Random.Range(baseRate * 0.8f, baseRate * 1.2f));
            PlaySound(soundName, 0.5f, 0.1f);
        }
    }

    // Vol should be between 0 and 1
    public void SetMusic(float MusicVol, float DarkVol, float ForebodingVol, float WindVol)
    {
        musicLayers[0].volume = MusicVol;
        musicLayers[1].volume = DarkVol;
        musicLayers[2].volume = ForebodingVol;
        musicLayers[3].volume = WindVol;
    }

    // RandomDepth should be between 0 and 1
    public void PlaySound(int soundToPlay, float volume = 0, float PitchRandDepth = 0)
    {
        audioSource.pitch = UnityEngine.Random.Range(1 - PitchRandDepth, 1 + PitchRandDepth);
        audioSource.PlayOneShot(sounds[soundToPlay].audioClip, volume);
    }

    public void PlaySound(string soundToPlay, float volume = 0, float PitchRandDepth = 0)
    {
        AudioClip audioClip = sounds.First(x => x.name.ToLower() == soundToPlay.ToLower()).audioClip;

        audioSource.pitch = UnityEngine.Random.Range(1 - PitchRandDepth, 1 + PitchRandDepth);
        audioSource.PlayOneShot(audioClip, volume);
    }
}
