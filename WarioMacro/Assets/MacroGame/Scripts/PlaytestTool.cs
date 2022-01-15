using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlaytestTool : MonoBehaviour
{
    [SerializeField] private string[] ScenesList = null;
    [SerializeField] private GameObject ScrollContent;
    [SerializeField] private GameObject ToggleTemplate;
    [SerializeField] private GameObject PlaytestPanel;
    private bool panelIsActive = false;
    private List<Toggle> TogglesList;
    [SerializeField] private GameController GameControl;
    [SerializeField] private ScenesReferencesSO ScenesRefs;

    private void Start()
    {
        InitSceneList();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            panelIsActive = !panelIsActive;
            PlaytestPanel.SetActive(panelIsActive);
        }
    }

    public void InitSceneList()
    {
        TogglesList = new List<Toggle>();

        List<String> SNames = new List<string>();
        foreach (MiniGameScriptableObject MGSO in ScenesRefs.MiniGames)
        {
            SNames.Add(MGSO.MiniGameScene.name);
        }

        GameControl.sceneNames = SNames.ToArray();
            
        ScenesList = GameControl.sceneNames;

        for (int i = 0; i < ScenesList.Length; i++)
        {
            GameObject go = Instantiate(ToggleTemplate, ScrollContent.transform);
            go.GetComponentInChildren<Text>().text = ScenesList[i];

            var i1 = i;
            go.GetComponentInChildren<Toggle>().onValueChanged.AddListener((value) =>
            {
                OnListUpdate(i1, value);
            });
            
            TogglesList.Add(go.GetComponentInChildren<Toggle>());
        }

        if (ToggleTemplate != null)
        {
            Destroy(ToggleTemplate);
            ToggleTemplate = null;
        }
    }

    public void OnListUpdate(int toggleIndex, bool isActivated)
    {
        Debug.Log(toggleIndex);
        List<String> SNames = new List<string>();
        SNames = GameControl.sceneNames.ToList();
        
        if (isActivated)
        {
            SNames.Add(ScenesList[toggleIndex]);
            GameControl.sceneNames = SNames.ToArray();
        }
        else
        {
            for (int i = 0; i < SNames.Count; i++)
            {
                if (SNames[i] == ScenesList[toggleIndex])
                {
                    SNames.RemoveAt(i);
                    GameControl.sceneNames = SNames.ToArray();
                    Debug.Log("La scene " + ScenesList[toggleIndex] +" n'est plus active");
                    return;
                }
            }
        }
        
        
        
        
        
    }
    
    

    public void OpenScenesList()
    {
        
    }

    public void CopyDebug()
    {
        
    }

    public void OpenExcel()
    {
        
    }

    public void TakeScreenshot()
    {
        
    }
}
