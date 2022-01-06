using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundList", menuName = "MacroGame/SoundList", order = 0)]
public class SoundsListSO : ScriptableObject
{
    public SoundTemp[] Sounds;

    public AudioClip FindSound(string name)
    {
        foreach (SoundTemp s in Sounds)
        {
            if (s.clipName == name) return s.clip;
        }
        Debug.Log("Aucun son nommé " + name + " n'a été trouvé.");
        return null;
    }
}

[System.Serializable]
public class SoundTemp
{
    public string clipName;
    public AudioClip clip;
}


