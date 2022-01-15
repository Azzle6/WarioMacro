using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NAA_SoundSample", menuName = "ScriptableObjects/MicroGameData/NAA_ChainedFist_Sound", order = 2)]
public class NAA_SoundTemplate_LAC : ScriptableObject
{
    public AudioClip audioclip;
    [Range(0, 1)]
    public float volume;
    public bool loop;
    [HideInInspector]
    public bool isPlaying;
}
