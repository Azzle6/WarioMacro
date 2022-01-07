using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "MusicManagerSO", menuName = "MacroGame/MusicManagerSO", order = 0)]
public class MusicManagerSO : ScriptableObject
{
    [FormerlySerializedAs("MusicsList")] public List<Soundgroup> MusicList = new List<Soundgroup>(2);

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
        ESCAPE 
    } 
     
    public PhaseState musicState = PhaseState.MACROGAME; 
    public CurrentPhase musicPhase = CurrentPhase.RECRUIT; 
    public List<SoundRef> sounds = new List<SoundRef>(8); 
 
 
} 
 
[System.Serializable] 
public class SoundRef 
{ 
    public int BPM = 120; 
    public AudioClip Clip; 
} 
