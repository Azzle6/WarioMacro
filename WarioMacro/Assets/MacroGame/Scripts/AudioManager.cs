using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;

    // public List<AudioClip> musicClips;
    // public List<AudioClip> soundClips;

    [SerializeField] private GameObject musicSourcesGO;
    [SerializeField] private GameObject soundSourcesGO;
    private AudioSource[] musicSources;
    private AudioSource[] soundSources;
    private int currentSoundSourceID;
    private int currentMusicSourceID;
    private int unavailableAudioCount;

    /// <summary>
    /// Returns the static instance of AudioManager. Creates a new one if not set.
    /// </summary>
    /// <returns>the instance</returns>
    public static void Register()
    {
        if (instance != null) return;

        var go = new GameObject("Audio Manager");
        go.AddComponent<AudioManager>();

        // instance.musicClips = new List<AudioClip>();
        // instance.soundClips = new List<AudioClip>();

        instance.musicSourcesGO = new GameObject("Music Sources");
        instance.musicSourcesGO.transform.SetParent(go.transform);
        instance.soundSourcesGO = new GameObject("Sound Sources");
        instance.soundSourcesGO.transform.SetParent(go.transform);

        for (int i = 0; i < 5; i++)
        {
            instance.musicSourcesGO.AddComponent<AudioSource>();
        }

        for (int i = 0; i < 16; i++)
        {
            instance.soundSourcesGO.AddComponent<AudioSource>();
        }

        instance = go.GetComponent<AudioManager>();
        instance.musicSources = instance.musicSourcesGO.GetComponents<AudioSource>();
        instance.soundSources = instance.soundSourcesGO.GetComponents<AudioSource>();
    }

    /*
    /// <summary>
    /// Method used to play a music. Don't use this one during a micro game. Use PlayMusic(AudioClip musicClip) instead.
    /// </summary>
    /// <param name="musicId"></param>
    public void PlayMusic(int musicId) => PlayAudio(musicClips[musicId], AudioType.MUSIC);
    
    /// <summary>
    /// Method used to play a sound. Don't use this one during a micro game. Use PlaySound(AudioClip soundClip) instead.
    /// </summary>
    /// <param name="soundId"></param>
    public void PlaySound(int soundId) => PlayAudio(soundClips[soundId], AudioType.SOUND);
    */

    /*
        /// <summary>
        /// Method used to instantly play a music.
        /// </summary>
        /// <param name="musicClip">the music you want to play</param>
        public static void PlayMusic(AudioClip musicClip) => instance.StartCoroutine(instance.PlayAudio(musicClip, AudioType.Music, 0f));

        /// <summary>
        /// Method used to play a music. Delay is measured in seconds and will delay the start by its value
        /// </summary>
        /// <param name="musicClip">the music you want to play</param>
        /// <param name="delay">the delay before the music starts playing</param>
        public static void PlayMusic(AudioClip musicClip, float delay) => instance.StartCoroutine(instance.PlayAudio(musicClip, AudioType.Music, delay));
    */

    /// <summary>
    /// Method used to instantly play a sound.
    /// </summary>
    /// <param name="soundClip">the sound you want to play</param>
    public static void PlaySound(AudioClip soundClip) => instance.StartCoroutine(instance.PlayAudio(soundClip, AudioType.Sound, 1f, 0f));

    /// <summary>
    /// Method used to instantly play a sound. Volume must be a float between 0 and 1.
    /// </summary>
    /// <param name="soundClip">the sound you want to play</param>
    /// <param name="volume">the volume of the sound</param>
    public static void PlaySound(AudioClip soundClip, float volume) => instance.StartCoroutine(instance.PlayAudio(soundClip, AudioType.Sound, volume, 0f));
    
    /// <summary>
    /// Method used to play a sound. Volume must be a float between 0 and 1. Delay is measured in seconds and will delay
    /// the start by its value.
    /// </summary>
    /// <param name="soundClip">the sound you want to play</param>
    /// <param name="volume">the volume of the sound</param>
    /// <param name="delay">the delay before the music starts playing</param>
    public static void PlaySound(AudioClip soundClip, float volume, float delay) => instance.StartCoroutine(instance.PlayAudio(soundClip, AudioType.Sound, volume, delay));

    /*
        /// <summary>
        /// Method used to instantly stop a music.
        /// </summary>
        /// <param name="musicClip">the music you want to stop</param>
        public static void StopMusic(AudioClip musicClip) => instance.StartCoroutine(instance.StopAudio(musicClip, AudioType.Music, 0f));

        /// <summary>
        /// Method used to stop a music. Delay is measured in seconds and will delay the end by its value
        /// </summary>
        /// <param name="musicClip">the music you want to stop</param>
        /// <param name="delay">the delay before the music stops playing</param>
        public static void StopMusic(AudioClip musicClip, float delay) => instance.StartCoroutine(instance.StopAudio(musicClip, AudioType.Music, delay));
    */

    /// <summary>
    /// Method used to instantly stop a sound.
    /// </summary>
    /// <param name="soundClip">the sound you want to stop</param>
    public static void StopSound(AudioClip soundClip) => instance.StartCoroutine(instance.StopAudio(soundClip, AudioType.Sound, 0f));

    /// <summary>
    /// Method used to stop a sound. Delay is measured in seconds and will delay the end by its value
    /// </summary>
    /// <param name="soundClip">the sound you want to stop</param>
    /// <param name="delay">the delay before the music stops playing</param>
    public static void StopSound(AudioClip soundClip, float delay) => instance.StartCoroutine(instance.StopAudio(soundClip, AudioType.Sound, delay));



    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        musicSources = musicSourcesGO.GetComponents<AudioSource>();
        soundSources = soundSourcesGO.GetComponents<AudioSource>();
    }

    private IEnumerator PlayAudio(AudioClip audioClip, AudioType type, float volume, float delay)
    {
        if (volume < 0 || volume > 1)
        {
            throw new ArgumentException("Volume must be an integer between 0 and 1.");
        }
        
        yield return new WaitForSeconds(delay);

        switch (type)
        {
            case AudioType.Music:
                PlayAudio(audioClip, volume, musicSources, ref currentMusicSourceID);
                break;
            case AudioType.Sound:
                PlayAudio(audioClip, volume, soundSources, ref currentSoundSourceID);
                break;
        }
    }

    [SuppressMessage("ReSharper", "TailRecursiveCall")]
    private void PlayAudio(AudioClip audioClip, float volume, IReadOnlyList<AudioSource> audioSources, ref int currentID)
    {
        AudioSource audioSource = audioSources[currentID];
        if (!audioSource.isPlaying)
        {
            audioSource.clip = audioClip;
            audioSource.volume = volume;
            audioSource.Play();

            unavailableAudioCount = 0;

            return;
        }

        currentID++;
        unavailableAudioCount++;

        if (currentID > audioSources.Count - 1)
        {
            currentID = 0;
        }

        if (unavailableAudioCount > audioSources.Count)
        {
            audioSource.Stop();
        }

        PlayAudio(audioClip, volume, audioSources, ref currentID);
    }

    private IEnumerator StopAudio(AudioClip audioClip, AudioType type, float delay)
    {
        yield return new WaitForSeconds(delay);

        switch (type)
        {
            case AudioType.Music:
                StopAudio(audioClip, musicSources);
                break;
            case AudioType.Sound:
                StopAudio(audioClip, soundSources);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }

    private static void StopAudio(AudioClip audioClip, IEnumerable<AudioSource> audioSources)
    {
        foreach (AudioSource audioSource in audioSources)
        {
            if (audioSource.isPlaying && audioSource.clip == audioClip)
            {
                audioSource.Stop();
                return;
            }
        }
    }

    private enum AudioType
    {
        Music,
        Sound
    }
}
