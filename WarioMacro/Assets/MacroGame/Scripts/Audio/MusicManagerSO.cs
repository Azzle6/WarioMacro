using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "MusicManagerSO", menuName = "MacroGame/MusicManagerSO", order = 0)]
public class MusicManagerSO : ScriptableObject
{
    public List<Soundgroup> MusicList = new List<Soundgroup>(2);

}

[System.Serializable] 
public class Soundgroup 
{ 
    public enum PhaseState 
    { 
        MACROGAME, 
        MINIGAME 
    } 
    public enum CurrentPhase 
    { 
        RECRUIT, 
        ACTION, 
        ESCAPE,
        MENU
    } 
     
    public PhaseState musicState = PhaseState.MACROGAME; 
    public CurrentPhase musicPhase = CurrentPhase.RECRUIT; 
    public List<SoundRef> sounds = new List<SoundRef>(8); 
 
 
} 
 
[System.Serializable] 
public class SoundRef 
{ 
    public int BPM = 100; 
    public AudioClip Clip; 
    [Range(0,2)]
    public float musicVolume = 1;
} 
