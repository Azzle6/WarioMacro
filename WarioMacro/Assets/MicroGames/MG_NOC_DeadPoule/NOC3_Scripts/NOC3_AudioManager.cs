using UnityEngine;

public class NOC3_AudioManager : MonoBehaviour
{
    void Start()
    {
        AudioManager.Register();
    }

    public static void PlaySound(AudioClip clip, float volume, float delay = 0f)
    {
        AudioManager.PlaySound(clip, volume, delay);
    }
}
