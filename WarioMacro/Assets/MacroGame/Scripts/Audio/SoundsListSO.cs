using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "SoundList", menuName = "MacroGame/SoundList", order = 0)]
public class SoundsListSO : ScriptableObject
{
    [FormerlySerializedAs("Sounds")] public SoundInfo[] sounds;

    public SoundInfo FindSound(string soundName)
    {
        foreach (SoundInfo s in sounds)
        {
            if (s.clipName == soundName) return s;
        }
        Debug.Log("No sound named " + soundName + ".");
        return null;
    }
}

[System.Serializable]
public class SoundInfo
{
    public string clipName;
    public AudioClip clip;
    public float clipVolume = 1;
}


