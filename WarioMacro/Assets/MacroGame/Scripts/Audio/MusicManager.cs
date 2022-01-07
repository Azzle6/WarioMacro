using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using UnityEngine;
// ReSharper disable InconsistentNaming 
 
public class MusicManager : MonoBehaviour, ITickable
{
    public static MusicManager instance;
    
    public AudioSource AudioS; 
    public AudioClip currentAudioClip; 
    private AudioClip nextAudioClip;
    public MusicsManagerSO MusicsSO;
    public SoundsListSO SoundsList;
    private bool WaitForTick;
    
    [Header("Visualize music Timer")]
    [Range(0, 1f)] public float musicTime;
     
    [Header("Musics")]
    public List<Soundgroup> AllSoundsList;

    
    
    private void Start() 
    {
        GameManager.Register();
        instance = this;
        //GameController.Init(this);
        AllSoundsList = MusicsSO.MusicsList;
        
        AudioS.loop = true;
        
        //Juste pour tester
        if (currentAudioClip != null)
        {
            AudioS.clip = currentAudioClip;
            return;
        }
        
        SwitchMusic(100, Soundgroup.PhaseState.MINIGAME, Soundgroup.CurrentPhase.RECRUIT);
        //StartCoroutine("CallSwitch"); //Juste pour tester
        
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
        
        //trouver un moyen de add l'audio source + ajouter le Scriptable Object
    }

    private void Update() //L'update est juste pour afficher le slider
    {
        if(AudioS.clip != null)
            musicTime = AudioS.time / AudioS.clip.length; 
    }

    IEnumerator CallSwitch() // juste pour tester
    {
        yield return new WaitForSeconds(3);
        SwitchMusic(140, Soundgroup.PhaseState.MACROGAME, Soundgroup.CurrentPhase.RECRUIT);
        yield return new WaitForSeconds(6);
        SwitchMusic(100, Soundgroup.PhaseState.MINIGAME, Soundgroup.CurrentPhase.RECRUIT);
    }
    
    


    public void SwitchMusic(int newBPM, Soundgroup.PhaseState newState, Soundgroup.CurrentPhase newPhase) 
    { 
        
        foreach (Soundgroup group in AllSoundsList) 
        { 
            if (group.musicPhase == newPhase && group.musicState == newState) 
            { 
                
                foreach (SoundRef sounds in group.sounds) 
                {
                    if (sounds.BPM == newBPM)
                    {
                        nextAudioClip = sounds.Clip;
                        WaitForTick = true;
                        //Debug.Log("nextAudioClip: " + nextAudioClip);
                        return;
                    } 
                } 
            } 
        }
        
        Debug.Log("Aucune musique correspondant à : " + newBPM + " " + newState + " " + newPhase);
    } 
 
    public void OnTick() 
    { 
        // TODO
        SwitchMusic((int) GameController.gameBPM, Soundgroup.PhaseState.MACROGAME, Soundgroup.CurrentPhase.RECRUIT);
        
        if (WaitForTick)
        {
            float nextTimer = 0;
            
            if (currentAudioClip != null)
            {
                nextTimer = ConvertMusicTimers();
            }
            
            AudioS.clip = nextAudioClip;
            Debug.Log(nextTimer);
            
            AudioS.time = nextTimer;
            AudioS.Play();
            
            currentAudioClip = nextAudioClip;
            nextAudioClip = null;
            WaitForTick = false;
        } 
    } 
 
    public float ConvertMusicTimers() 
    { 
        double percentage = AudioS.time / currentAudioClip.length; 
        Debug.Log(percentage);
        return (float) (nextAudioClip.length * percentage);
    } 
     
} 
 


