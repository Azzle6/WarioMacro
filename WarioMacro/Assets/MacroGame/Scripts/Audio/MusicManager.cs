using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

// ReSharper disable InconsistentNaming 
// ReSharper disable once CheckNamespace
public class MusicManager : MonoBehaviour, ITickable
{
    public static MusicManager instance;
    
    [Header("Visualize music Timer")] [Range(0, 1f)] public float musicTime;
    [Header("Musics")] public List<Soundgroup> AllSoundsList;
    [FormerlySerializedAs("MusicsSO")] public MusicManagerSO musicSO;
    public AudioSource AudioS; 
    public AudioClip currentAudioClip;
    public SoundsListSO SoundsList;
    
    private AudioClip nextAudioClip;
    private bool WaitForTick;
    

    private void Start() 
    {
        GameManager.Register();
        instance = this;
        AllSoundsList = musicSO.MusicList;
        
        AudioS.loop = true;

        SwitchMusic(100, Soundgroup.PhaseState.MINIGAME, Soundgroup.CurrentPhase.RECRUIT);

    }

    public void PlayASound(string clipName)
    {
        AudioManager.PlaySound(SoundsList.FindSound(clipName));
    }

    public static void Register()
    {
        if (instance != null) return;

        var go = new GameObject("Musics Manager");
        go.AddComponent<AudioManager>();
        
        //Trouver un moyen d'add l'audio source + ajouter le Scriptable Object
    }

    private void Update() //L'update est juste pour afficher le slider
    {
        if(AudioS.clip != null)
            musicTime = AudioS.time / AudioS.clip.length; 
    }

    private void SwitchMusic(int newBPM, Soundgroup.PhaseState newState, Soundgroup.CurrentPhase newPhase) 
    { 
        
        // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
        foreach (Soundgroup group in AllSoundsList)
        {
            if (@group.musicPhase != newPhase || @group.musicState != newState) continue;
            
            foreach (SoundRef sounds in @group.sounds.Where(sounds => sounds.BPM == newBPM))
            {
                nextAudioClip = sounds.Clip;
                WaitForTick = true;
                //Debug.Log("nextAudioClip: " + nextAudioClip);
                return;
            }
        }
        
        //Debug.Log("No music corresponding to : " + newBPM + " " + newState + " " + newPhase);
    } 
 
    public void OnTick() 
    { 
        // TODO
        SwitchMusic((int) GameController.gameBPM, Soundgroup.PhaseState.MACROGAME, Soundgroup.CurrentPhase.RECRUIT);

        if (!WaitForTick) return;
        
        float nextTimer = 0;
            
        if (currentAudioClip != null)
        {
            nextTimer = ConvertMusicTimers();
        }
            
        AudioS.clip = nextAudioClip;
        //Debug.Log(nextTimer);
            
        AudioS.time = nextTimer;
        AudioS.Play();
            
        currentAudioClip = nextAudioClip;
        nextAudioClip = null;
        WaitForTick = false;
    }

    private float ConvertMusicTimers() 
    { 
        double percentage = AudioS.time / currentAudioClip.length;
        return (float) (nextAudioClip.length * percentage);
    } 
     
} 
 


