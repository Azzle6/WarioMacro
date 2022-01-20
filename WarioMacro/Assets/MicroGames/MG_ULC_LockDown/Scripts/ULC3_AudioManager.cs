using System;
using System.Collections.Generic;
using UnityEngine;

public class ULC3_AudioManager : MonoBehaviour
{
    public static ULC3_AudioManager instance;

    [SerializeField] private AudioSource[] audioSources;
    [SerializeField] private List<AudioKey> audioKeys = new List<AudioKey>();

    [Serializable]
    public class AudioKey
    {
        public string key;
        public AudioClip clip;
        [Range(0,1f)] public float volume;
        public float soundOffset;
    }

    void Awake()
    {
        instance = this;
    }

    private AudioSource selectedAudioSrc;
    public void PlaySFX(string key, float pitch)
    {
        foreach (AudioKey ak in audioKeys)
        {
            if (ak.key == key)
            {
                selectedAudioSrc = SelectAudioSource();
                selectedAudioSrc.clip = ak.clip;
                selectedAudioSrc.volume = ak.volume;
                selectedAudioSrc.time = ak.soundOffset;

                selectedAudioSrc.pitch = pitch;
                
                selectedAudioSrc.Play();
                return;
            }
        }
    }

    public AudioSource SelectAudioSource()
    {
        foreach (AudioSource asrc in audioSources)
        {
            if (!asrc.isPlaying) return asrc;
        }

        return audioSources[0];
    }
}
