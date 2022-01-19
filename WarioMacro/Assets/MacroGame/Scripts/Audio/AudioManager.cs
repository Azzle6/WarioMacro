using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

[SuppressMessage("ReSharper", "SwitchStatementHandlesSomeKnownEnumValuesWithDefault")]
public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;

    public List<AudioClip> musicClips;

    [SerializeField] private GameObject musicSourcesGO;
    [SerializeField] private GameObject soundSourcesGO;
    [SerializeField] private GameObject mgSoundSourcesGO;
    [SerializeField] private SoundsListSO soundList;
    private AudioSource[] musicSources;
    private AudioSource[] soundSources;
    private AudioSource[] mgSoundSources;
    private int currentSoundSourceID;
    private int currentMGSoundSourceID;
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
        instance.mgSoundSourcesGO = new GameObject("Micro Games Sound Sources");
        instance.mgSoundSourcesGO.transform.SetParent(go.transform);

        for (int i = 0; i < 6; i++)
        {
            instance.musicSourcesGO.AddComponent<AudioSource>();
        }

        for (int i = 0; i < 9; i++)
        {
            instance.soundSourcesGO.AddComponent<AudioSource>();
        }
        
        for (int i = 0; i < 13; i++)
        {
            instance.mgSoundSourcesGO.AddComponent<AudioSource>();
        }

        instance = go.GetComponent<AudioManager>();
        instance.musicSources = instance.musicSourcesGO.GetComponents<AudioSource>();
        instance.soundSources = instance.soundSourcesGO.GetComponents<AudioSource>();
        instance.soundSources = instance.mgSoundSourcesGO.GetComponents<AudioSource>();

        instance.soundList = Resources.Load<SoundsListSO>("SoundListSO");
        
        // Adding Music Manger (must be called after initializing audio sources)
        go.AddComponent<MusicManager>();
    }

    
    /// <summary>
    /// Method used to play a music. Don't use this one during a micro game. Use PlayMusic(AudioClip musicClip) instead.
    /// </summary>
    /// <param name="musicId"></param>
    public static void MacroPlayMusic(int musicId) => instance.StartCoroutine(instance.PlayAudio(instance.musicClips[musicId], AudioType.Music, 1f, 0f));

    /// <summary>
    /// Method used to play a sound. Don't use this one during a micro game. Use PlaySound(AudioClip soundClip) instead.
    /// </summary>
    /// <param name="soundName"></param>
    public static void MacroPlaySound(string soundName) => MacroPlaySound(soundName, 0);

    /// <summary>
    /// Method used to play a sound. Don't use this one during a micro game. Use PlaySound(AudioClip soundClip) instead.
    /// </summary>
    /// <param name="soundName"></param>
    /// <param name="delay"></param>
    public static void MacroPlaySound(string soundName, float delay)
    {
        SoundInfo sound = instance.soundList.FindSound(soundName);
        instance.StartCoroutine(instance.PlayAudio(sound.clip, AudioType.Sound, sound.clipVolume, delay));
    }
    
    /// <summary>
    /// Method used to play a sound. Don't use this one during a micro game. Use PlaySound(AudioClip soundClip) instead.
    /// </summary>
    /// <param name="soundName"></param>
    /// <param name="delay"></param>
    public static void MacroPlayRandomSound(string soundName, float delay)
    {
        SoundInfo sound = instance.soundList.FindRandomSound(soundName);
        instance.StartCoroutine(instance.PlayAudio(sound.clip, AudioType.Sound, sound.clipVolume, delay));
    }

    /// <summary>
    /// Method used to stop a music. Delay is measured in seconds and will delay the end by its value
    /// </summary>
    /// <param name="musicClip">the music you want to stop</param>
    /// <param name="delay">the delay before the music stops playing</param>
    public static void StopMusic(AudioClip musicClip, float delay) => instance.StartCoroutine(instance.StopAudio(musicClip, AudioType.Music, delay));

    /// <summary>
    /// Method used to stop a music. Delay is measured in seconds and will delay the end by its value
    /// </summary>
    /// <param name="soundName"></param>
    /// <param name="delay">the delay before the music stops playing</param>
    public static void StopMacroSound(string soundName, float delay) => instance.StartCoroutine(instance.StopAudio(instance.soundList.FindSound(soundName).clip, AudioType.Sound, delay));

    public static void StopAllMicroSounds()
    {
        foreach (AudioSource audioSource in instance.mgSoundSources)
        {
            audioSource.Stop();
        }
    }

    
    
    
    /// <summary>
    /// Method used to instantly play a sound.
    /// </summary>
    /// <param name="soundClip">the sound you want to play</param>
    public static void PlaySound(AudioClip soundClip) => instance.StartCoroutine(instance.PlayAudio(soundClip, AudioType.SoundMG, 1f, 0f));

    /// <summary>
    /// Method used to instantly play a sound. Volume must be a float between 0 and 1.
    /// </summary>
    /// <param name="soundClip">the sound you want to play</param>
    /// <param name="volume">the volume of the sound</param>
    public static void PlaySound(AudioClip soundClip, float volume) => instance.StartCoroutine(instance.PlayAudio(soundClip, AudioType.SoundMG, volume, 0f));
    
    /// <summary>
    /// Method used to play a sound. Volume must be a float between 0 and 1. Delay is measured in seconds and will delay
    /// the start by its value.
    /// </summary>
    /// <param name="soundClip">the sound you want to play</param>
    /// <param name="volume">the volume of the sound</param>
    /// <param name="delay">the delay before the music starts playing</param>
    public static void PlaySound(AudioClip soundClip, float volume, float delay) => instance.StartCoroutine(instance.PlayAudio(soundClip, AudioType.SoundMG, volume, delay));

    /// <summary>
    /// Method used to instantly stop a sound.
    /// </summary>
    /// <param name="soundClip">the sound you want to stop</param>
    public static void StopSound(AudioClip soundClip) => instance.StartCoroutine(instance.StopAudio(soundClip, AudioType.SoundMG, 0f));

    /// <summary>
    /// Method used to stop a sound. Delay is measured in seconds and will delay the end by its value
    /// </summary>
    /// <param name="soundClip">the sound you want to stop</param>
    /// <param name="delay">the delay before the music stops playing</param>
    public static void StopSound(AudioClip soundClip, float delay) => instance.StartCoroutine(instance.StopAudio(soundClip, AudioType.SoundMG, delay));



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
        mgSoundSources = mgSoundSourcesGO.GetComponents<AudioSource>();
        DontDestroyOnLoad(gameObject);
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
            case AudioType.SoundMG:
                PlayAudio(audioClip, volume, mgSoundSources, ref currentMGSoundSourceID);
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
            case AudioType.SoundMG:
                StopAudio(audioClip, mgSoundSources);
                break;
        }
    }

    // ReSharper disable once SuggestBaseTypeForParameter
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
        Sound,
        SoundMG
    }
}
