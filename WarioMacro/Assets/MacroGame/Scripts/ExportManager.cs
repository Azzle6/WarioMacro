using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEditor;
using UnityEditor.SearchService;
using UnityEngine.WSA;
using Object = UnityEngine.Object;
using Scene = UnityEngine.SceneManagement.Scene;

public class ExportManager : EditorWindow
{
    private string trigramme = "AAA";
    private string path = "Select folder";
    private string MiniGameName = "MiniGame name";
    private string[] ignoreNames = new string[] {"Resources", "Scripts"};
    private List<String> wrongNamesPaths = new List<string>();
    
    //List<String> foldersPath = new List<string>();

    enum NameType
    {
        SCENE,
        MAINFOLDER,
        ASSETS
    }


    [MenuItem("Assets/Export MiniGame")]
    private static void ExportationWindow()
    {
        EditorWindow.GetWindow(typeof(ExportManager));
        Debug.Log(("Exportation"));
    }
    
    //Fonction de validation : Conditions pour activer/désactiver le bouton
    [MenuItem("Assets/Export MiniGame", true)]
    private static bool ExportationWindowValidation()
    {
        return Selection.activeObject != null;
    }

    private bool VerifyNames(string itemName, NameType nt)
    {
        switch (nt)
        {
            case NameType.ASSETS :
                if (itemName.Substring(0, 4) == trigramme + "_") return true;
                break;
            
            case NameType.SCENE :

                if (itemName.Substring(0, 3) == "MG_" &&
                    itemName.Substring(3, 4) == trigramme + "_" &&
                    itemName.Substring(7, 6) == "Scene_" &&
                    itemName.Substring(13, MiniGameName.Length) == MiniGameName) return true;
                break;
            
            case NameType.MAINFOLDER :
                
                if (itemName.Substring(0, 3) == "MG_" &&
                    itemName.Substring(3, 4) == trigramme + "_" &&
                    itemName.Substring(7, MiniGameName.Length) == MiniGameName) return true;
                break;
        }
        return false;
    }
    
    

    private void OnGUI()
    {
        GUILayout.Label(("Exportation de Mini-jeux"), EditorStyles.boldLabel);
        
        EditorGUILayout.Space();
        //renseigner le trigramme
        GUILayout.Label(("Trigramme"));
        trigramme = EditorGUILayout.TextField("", trigramme);
        
        //Renseigner le nom du Mini jeu
        GUILayout.Label(("MiniGame name"));
        MiniGameName = EditorGUILayout.TextField("", MiniGameName);
        
        //Renseigner le path
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        GUILayout.Label(("Mini-Game Folder Path"));

        
        path = EditorGUILayout.TextField("", path);
        
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        
        if (GUILayout.Button("Update Folder Path"))
        {
            UpdatePathField();
        }

        if (GUILayout.Button("Check names"))
        {
            CheckExportConditions();
        }

        if (GUILayout.Button("Export"))
        {
            ExportMiniGame();
        }
        
    }

    public void UpdatePathField()
    {
        if (Selection.activeObject != null)
        {
            path = AssetDatabase.GetAssetPath(Selection.activeObject.GetInstanceID());
        }
    }

    

    public void CheckExportConditions()
    {
        //référencer tous les éléments des dossiers
        string[] pathArray = new string[]{path};
        List<String> allPathUUIDs = AssetDatabase.FindAssets("", pathArray).ToList();
        List<String> allAssetsNames = new List<string>();
        
        
        //Détection des scenes
        
        String[] ScenePaths = AssetDatabase.FindAssets("t:scene", pathArray);
        
        if (ScenePaths.Length == 0)
        {
            Debug.Log("Pas de scene dans le dossier !");
            return;
        }
        else if (ScenePaths.Length > 1)
        {
            Debug.Log("Plusieurs scenes dans le dossier !");
            return;
        }
        
        
        //Tri des différents assets
        for (int i = 0; i < allPathUUIDs.Count; i++)
        {
            string currentAssetName = Path.GetFileNameWithoutExtension(AssetDatabase.GUIDToAssetPath(allPathUUIDs[i]));
            
            //Vérifie si c'est la scene
            if (AssetDatabase.GUIDToAssetPath(allPathUUIDs[i]) == AssetDatabase.GUIDToAssetPath(ScenePaths[0]))
            {
                //Debug.Log("remove scene from list");
                allPathUUIDs.RemoveAt(i);
                i--;
            }
            else //Verifie si l'asset est dans la liste des assets ignorés
            {
                for (int e = 0; e < ignoreNames.Length; e++)
                {
                    
                    if (ignoreNames[e] == currentAssetName)
                    {
                        //Debug.Log("remove asset from list");
                        allPathUUIDs.RemoveAt(i);
                        i--;
                    }
                }
            }
            
        }
        
        

        //Récupération des noms
        
        String SceneName = Path.GetFileNameWithoutExtension(AssetDatabase.GUIDToAssetPath(ScenePaths[0]));

        String FolderName = Path.GetFileName(path);
        
        foreach (var UIDs in allPathUUIDs)
        {
            allAssetsNames.Add(Path.GetFileNameWithoutExtension(AssetDatabase.GUIDToAssetPath(UIDs)));
        }
        
        //Vérification des noms
        bool canExport = true;

        if (!VerifyNames(FolderName, NameType.MAINFOLDER))
        {
            Debug.Log(FolderName + " est mal nommé");
            canExport = false;
        }

        if (!VerifyNames(SceneName, NameType.SCENE))
        {
            Debug.Log(SceneName + " est mal nommée");
            canExport = false;
        }

        foreach (var assetName in allAssetsNames)
        {
            if (!VerifyNames(assetName, NameType.ASSETS))
            {
                Debug.Log(assetName + " est mal nommé");
                canExport = false;
            }
        }
        
        if(canExport) Debug.Log("Tous les noms sont à jour, l'export est possible !");
        else Debug.Log("Export non possible, corrigez les noms.");
    }

    private void ExportMiniGame()
    {
        //if(!Directory.Exists("MG_Packages")) Directory.CreateDirectory("MG_Packages");
        string exportPath = EditorUtility.OpenFolderPanel("Select export location", "", "");
        
        if(exportPath.Length != 0) AssetDatabase.ExportPackage(path, exportPath + "/" + trigramme + "_" +MiniGameName + "_MGpackage.unitypackage", ExportPackageOptions.Interactive | ExportPackageOptions.Recurse | ExportPackageOptions.Default);
    }
    
}
