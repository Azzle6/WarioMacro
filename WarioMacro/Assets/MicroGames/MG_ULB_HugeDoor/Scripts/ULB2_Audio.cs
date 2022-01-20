using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class ULB2_Audio : MonoBehaviour
{
    public static ULB2_Audio instance;
    public AudioSource[] audio;
    public AudioMixer audioMixer;

    // Start is called before the first frame update
    void Start()
    {
        if (ULB2_Audio.instance != null)
        {
            Destroy(this);
        }

        instance = this;
    }

    
}
