using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ULA1_SoundManager : MonoBehaviour
{
    public static ULA1_SoundManager instance;
    public AudioSource[] audioSources;
    public AudioClip[] audioClips;

    // Start is called before the first frame update
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySound(int soundId)
    {
        audioSources[soundId].clip = audioClips[soundId];
        audioSources[soundId].Play();
    }

    public void PlaySoundDelay(int soundId)
    {
        audioSources[soundId].clip = audioClips[soundId];
        StartCoroutine(CoroutinePlaySound(soundId));
    }

    IEnumerator CoroutinePlaySound(int soundId)
    {
        yield return new WaitForSeconds(1f);
        audioSources[soundId].Play();
    }

    public void StopAll()
    {
        for (int i = 0; i < audioSources.Length; i++)
        {
            audioSources[i].Stop();
        }
    }

}