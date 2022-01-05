using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using UnityEngine;
// ReSharper disable InconsistentNaming 
 
public class MusicManager : MonoBehaviour, ITickable
{
    private static MusicManager instance;
    
    public AudioSource AudioS; 
    public AudioClip currentAudioClip; 
    private AudioClip nextAudioClip;
    public MusicsManagerSO MusicsSO;
    private bool WaitForTick;
    
    [Header("Visualize music Timer")]
    [Range(0, 1f)] public float musicTime;
     
    [Header("Musics")]
    public List<Soundgroup> AllSoundsList;

    
    
    private void Start() 
    {
        GameManager.Register();
        GameController.Init(this);
        AllSoundsList = MusicsSO.MusicsList;
        
        AudioS.loop = true;
        
        //Juste pour tester
        if (currentAudioClip != null)
        {
            AudioS.clip = currentAudioClip;
            return;
        }
        
        SwitchMusic(100, Soundgroup.PhaseState.MINIGAME, Soundgroup.CurrentPhase.RECRUIT);
        StartCoroutine("CallSwitch"); //Juste pour tester

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
 


