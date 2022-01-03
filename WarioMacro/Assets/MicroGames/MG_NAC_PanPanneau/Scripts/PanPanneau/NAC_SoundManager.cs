using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NAC_SoundManager : MonoBehaviour
{
    [SerializeField] private List<AudioClip> audioClips = new List<AudioClip>();
    [SerializeField] private AudioSource[] audioSources = new AudioSource[4];
    private AudioSource audioSource;

    public void PlayShotSound() 
    {
        audioSource = FindAudioSource();
        audioSource.clip = audioClips[0];
        audioSource.volume = 0.3f;
        audioSource.Play();
    }

    private AudioSource FindAudioSource() 
    {
        foreach (var audioSource in audioSources)
        {
            if (!audioSource.isPlaying) 
            {
                return audioSource;
            }
        }
        return audioSources[1];
    }
}
