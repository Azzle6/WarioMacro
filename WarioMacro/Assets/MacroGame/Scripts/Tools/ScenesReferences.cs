using System.Collections.Generic;
using System.Linq;
using UnityEngine;



using UnityEditor;

[ExecuteInEditMode]
public class ScenesReferences : MonoBehaviour
{
    public ScenesReferencesSO scenesRefs;
    
    public void SearchAllScenes()
    {
        string[] idList = scenesRefs.MiniGames.Select(mg => AssetDatabase.GetAssetPath(mg.MiniGameScene)).ToArray();

        foreach (var id in idList)
        {
            AddSceneToBuild(id);
        }
    }
    
    public static void AddSceneToBuild(string path)
    {
        var scenes = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);

        if (scenes.Any(buildSett => buildSett.path == path)) return;

        var newScene = new EditorBuildSettingsScene(path, true);
        scenes.Add(newScene);
        EditorBuildSettings.scenes = scenes.ToArray();
        Debug.Log(path + " registered.");
    }
}
