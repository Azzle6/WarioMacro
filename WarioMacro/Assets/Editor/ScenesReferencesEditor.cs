using System;
using System.Collections;
using System.Collections.Generic;
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
        //SerializedProperty pathProperties = serializedObject.FindProperty("MiniGameScenesPath");
        
        EditorGUILayout.LabelField("Mini Game Scenes Path :");
        script.MiniGameScenesPath = EditorGUILayout.TextField("", script.MiniGameScenesPath);
        EditorGUILayout.Space();
        
        if (GUILayout.Button("Register scenes"))
        {
            script.SearchAllScenes();
        }
        
        if (GUILayout.Button(("Add this scene to build")))
        {
            script.AddSceneToBuild(SceneManager.GetActiveScene().path);
        }

        serializedObject.ApplyModifiedProperties();

    }
}
