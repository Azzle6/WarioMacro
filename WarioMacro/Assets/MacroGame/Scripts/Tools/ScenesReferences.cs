using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using System.IO;
using UnityEditor;

[ExecuteInEditMode]
public class ScenesReferences : MonoBehaviour
{
    public ScenesReferencesSO scenesRefs;
    
    public void SearchAllScenes()
    {
        string[] idList = scenesRefs.MiniGames.Select(mg => AssetDatabase.GetAssetPath(mg.MiniGameScene)).ToArray();

        for (var index = 0; index < idList.Length; index++)
        {
            var id = idList[index];
            AddSceneToBuild(id);
            AddSceneNameToMG(Path.GetFileNameWithoutExtension(id), index);
        }
    }
    
    private void AddSceneNameToMG(string id, int i)
    {
        scenesRefs.MiniGames[i].MiniGameSceneName = id;
        Debug.Log(id);
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

#endif