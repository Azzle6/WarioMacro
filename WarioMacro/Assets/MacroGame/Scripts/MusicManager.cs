using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using UnityEngine;
// ReSharper disable InconsistentNaming 
 
public class MusicManager : MonoBehaviour, ITickable
{
    public AudioSource AudioS; 
    public AudioClip currentAudioClip; 
    private AudioClip nextAudioClip; 
    private bool WaitForTick;
    
    [Header("Visualize music Timer")]
    [Range(0, 1f)] public float musicTime;
     
    [Header("Musics")]
    public List<Soundgroup> AllSoundsList = new List<Soundgroup>(2);

    
    
    private void Start() 
    {
        GameManager.Register();
        GameController.Init(this);
        AudioS.loop = true;
        
        //Juste pour tester
        if (currentAudioClip != null)
        {
            AudioS.clip = currentAudioClip;
            return;
        }
        
        SwitchMusic(100, Soundgroup.PhaseState.MINIGAME, Soundgroup.CurrentPhase.RECRUIT);
        StartCoroutine("CallSwitch");

    }

    private void Update() //L'update est juste pour afficher le slider
    {
        musicTime = AudioS.time / AudioS.clip.length; 
    }

    IEnumerator CallSwitch() // juste pour tester
    {
        yield return new WaitForSeconds(6);
        SwitchMusic(110, Soundgroup.PhaseState.MACROGAME, Soundgroup.CurrentPhase.RECRUIT);
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
                        return;
                    } 
                } 
            } 
        }
        
        Debug.Log("Aucune musique correspondant à : " + newBPM + " " + newState + " " + newPhase);
    } 
 
    public void OnTick() 
    { 
        if (WaitForTick)
        {
            float nextTimer = 0;
            
            if (currentAudioClip != null)
            {
                nextTimer = ConvertMusicTimers();
            }
            
            AudioS.clip = nextAudioClip;
            AudioS.Play();
            AudioS.time = nextTimer;
            
            currentAudioClip = nextAudioClip;
            nextAudioClip = null;
            WaitForTick = false; 
        } 
    } 
 
    public float ConvertMusicTimers() 
    { 
        float percentage = AudioS.time / AudioS.clip.length; 
        return nextAudioClip.length * percentage; 
    } 
     
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

