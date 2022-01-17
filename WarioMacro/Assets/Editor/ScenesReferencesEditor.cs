using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;


[CustomEditor(typeof(ScenesReferences))]
public class ScenesReferencesEditor : Editor
{
    private ScenesReferences script;

    private void OnEnable()
    {
        script = (ScenesReferences) target;
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField("Mini Game Scenes References :");
        script.scenesRefs = EditorGUILayout.ObjectField(script.scenesRefs, typeof(ScenesReferencesSO), false) as ScenesReferencesSO;
        EditorGUILayout.Space();
        
        if (GUILayout.Button("Register scenes"))
        {
            script.SearchAllScenes();
        }
        
        if (GUILayout.Button(("Add this scene to build")))
        {
            ScenesReferences.AddSceneToBuild(SceneManager.GetActiveScene().path);
        }

        serializedObject.ApplyModifiedProperties();

    }
}
