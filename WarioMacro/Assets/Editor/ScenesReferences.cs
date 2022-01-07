using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class ScenesReferences : MonoBehaviour
{
    public string MiniGameScenesPath = "Assets";
    public void SearchAllScenes()
    {
        string[] searchAt = new string[] {MiniGameScenesPath};
        List<String[]> IDs = new List<string[]>();

        string[] IDStrings = AssetDatabase.FindAssets("t:scene", searchAt);
        

        foreach (var IDsList in IDStrings)
        {
            AddSceneToBuild(AssetDatabase.GUIDToAssetPath(IDsList));
        }
    }
    
    public void AddSceneToBuild(string path)
    {
        List<EditorBuildSettingsScene> scenes = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
        

        foreach (EditorBuildSettingsScene buildSett in scenes)
        {
            if (buildSett.path == path)
            {
                Debug.Log("Scene already register");
                return;
            }
            
        }
        
        EditorBuildSettingsScene newScene = new EditorBuildSettingsScene(path, true);
        scenes.Add((newScene));
        EditorBuildSettings.scenes = scenes.ToArray();
        Debug.Log("register : " + path);
    }
}
