using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using UnityEditor.Audio;

public class SoundMixer : EditorWindow
{
    
    private SoundsListSO SoundsSO;
    private MusicManagerSO MusicsSO;

    private GameObject PlaytestGO;

    private AudioSource[] AudioS;
    
    List<string> SoundsNamesList;
    List<string> MusicsNamesList;
    
    int selectedSound = 0;
    int selectedMusic = 0;

    
    
    [MenuItem("Assets/Sound Mixer")]
    private static void SoundWindow()
    {
        EditorWindow.GetWindow(typeof(SoundMixer));
        EditorWindow.GetWindow<SoundMixer>("OnDestroy");
    }

    private void Init()
    {
        CreateObjects();
        Debug.Log("OnEnable");
    }


    private void OnGUI()
    {
        if(PlaytestGO == null) Init();
        MusicsSO = (MusicManagerSO)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets("t:MusicManagerSO")[0]), typeof(MusicManagerSO));
        SoundsSO = (SoundsListSO)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets("t:SoundsListSO")[0]), typeof(SoundsListSO));
        
        MusicsSO = (MusicManagerSO)EditorGUILayout.ObjectField(MusicsSO, typeof(MusicManagerSO), true);
        SoundsSO = (SoundsListSO)EditorGUILayout.ObjectField(SoundsSO, typeof(SoundsListSO), true);
        
        
        if(SoundsNamesList == null && SoundsSO != null) RegisterSounds();
        if(MusicsNamesList == null && MusicsSO != null) RegisterMusics();
        
        EditorGUILayout.Space();

        if (AudioS != null && PlaytestGO != null)
        {
            if (MusicsNamesList != null && MusicsSO != null)
            {
                GUILayout.Label("Music Clip");
                //Sélection d'un nom de musique
                selectedMusic = EditorGUILayout.Popup("", selectedMusic, MusicsNamesList.ToArray());
                SoundRef musicInf = SearchMusicInfo(MusicsNamesList[selectedMusic]);

                SearchMusicInfo(MusicsNamesList[selectedMusic]).musicVolume =
                    EditorGUILayout.Slider(SearchMusicInfo(MusicsNamesList[selectedMusic]).musicVolume, 0, 1);
            
                //Retrouve le clip associé à ce nom


                if (GUILayout.Button("Play Music"))
                {
                    
                    PlaySound(musicInf.Clip, 0, musicInf.musicVolume);
                }

                if (GUILayout.Button("Stop Music")) StopAudio(0);
            }
        
            EditorGUILayout.Space();
        
            EditorGUILayout.Space();
        
            if (SoundsNamesList != null && SoundsSO != null)
            {
                GUILayout.Label("VFX Clip");
        
                selectedSound = EditorGUILayout.Popup("", selectedSound, SoundsNamesList.ToArray());
                SoundInfo soundInf = SearchSoundInfo(SoundsNamesList[selectedSound]);

                SearchSoundInfo(SoundsNamesList[selectedSound]).clipVolume =
                    EditorGUILayout.Slider(soundInf.clipVolume, 0, 1);
                
                if (GUILayout.Button("Play Sound"))
                {
                    
                    PlaySound(soundInf.clip, 1, soundInf.clipVolume);
                }
                if(GUILayout.Button("Stop Sound")) StopAudio(1);
                
            
            }
            EditorGUILayout.Space();
            if(GUILayout.Button("Stop All Sounds")) StopAll();
            
            
        }
    }


    private void CreateObjects()
    {
        GameObject testGO = new GameObject("test");
        
        for (int i = 0; i < 2; i++)
        {
            testGO.AddComponent<AudioSource>();
        }

        PlaytestGO = testGO;
        AudioS  = PlaytestGO.GetComponents<AudioSource>();
        
       

    }

    private void DestroyAudioSources()
    {
        if (PlaytestGO.GetComponents<AudioSource>() != null)
        {
            foreach (AudioSource AudioSo in PlaytestGO.GetComponents<AudioSource>())
            {
                DestroyImmediate(AudioSo);
            }
        }
    }
    

    private void RegisterMusics()
    {
        selectedMusic = 0;
        MusicsNamesList = new List<string>();

        foreach (Soundgroup musicsInfo in MusicsSO.MusicList)
        {
            foreach (SoundRef musics in musicsInfo.sounds)
            {
                MusicsNamesList.Add(musics.Clip.name);
            }
        }
        
    }

    private void RegisterSounds()
    {
        selectedSound = 0;
        SoundsNamesList = new List<string>();
        
        foreach (SoundInfo soundsInfo in SoundsSO.sounds)
        {
            SoundsNamesList.Add(soundsInfo.clipName);
        }

        foreach (SoundInfoList soundsInfoL in SoundsSO.randomSounds)
        {
            foreach (SoundInfo soundInfo in soundsInfoL.sounds)
            {
                SoundsNamesList.Add(soundInfo.clip.name);
            }
        }
    }

    private SoundInfo SearchSoundInfo(string clipName)
    {
        foreach (SoundInfo soundsInfo in SoundsSO.sounds)
        {
            if(soundsInfo.clipName == clipName) return soundsInfo;
        }

        foreach (SoundInfoList soundsInfoL in SoundsSO.randomSounds)
        {
            foreach (SoundInfo soundInfo in soundsInfoL.sounds)
            {
                if(soundInfo.clip.name == clipName) return soundInfo;
            }
        }

        return null;
    }

    private SoundRef SearchMusicInfo(string musicName)
    {
        foreach (Soundgroup musicsInfo in MusicsSO.MusicList)
        {
            foreach (SoundRef musics in musicsInfo.sounds)
            {
                if(musics.Clip.name == musicName) return musics;
            }
        }

        return null;
    }

    private void PlaySound(AudioClip clip, int audioIndex, float volume)
    {
        if (volume < 0 || volume > 1)
        {
            Debug.Log("Volume de " + clip.name + " incorrect.");
            return;
        }

        AudioS[audioIndex].clip = clip;
        AudioS[audioIndex].volume = volume;
        
        AudioS[audioIndex].Play();
    }

    private void StopAudio(int audioIndex)
    {
        AudioS[audioIndex].Stop();
    }

    private void StopAll()
    {
        foreach (var audio in AudioS)
        {
            audio.Stop();
        }
    }

    private void OnDestroy()
    {
        if (PlaytestGO != null)
        {
            StopAll();
            DestroyAudioSources();
            DestroyImmediate(PlaytestGO);
        }
        
        Debug.Log("destroyed");
    }

    /*
    public void PlaySound(AudioClip sound, int startSample = 0, bool loop = false)
    {
        Assembly unityEditorAssembly = typeof(AudioImporter).Assembly;
        Type audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
        MethodInfo method = audioUtilClass.GetMethod("PlayPreviewClip",
            BindingFlags.Static | BindingFlags.Public, null,
            new System.Type[] {typeof(AudioClip), typeof(int), typeof(bool)}, 
                null);

        method.Invoke(null, new object[] {sound, startSample, loop});
    }

    
    public void StopAllSound()
    {
        Assembly unityEditorAssembly = typeof(AudioImporter).Assembly;
 
        Type audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
        MethodInfo method = audioUtilClass.GetMethod(
            "StopAllPreviewClips",
            BindingFlags.Static | BindingFlags.Public,
            null,
            new Type[] {},
            null
        );
 
        method.Invoke(
            null,
            new object[] {}
        );
    }*/
}
