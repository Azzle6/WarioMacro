using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlaytestTool : MonoBehaviour
{
    [SerializeField] private string[] ScenesList = null;
    [SerializeField] private GameObject ScrollContent;
    [SerializeField] private GameObject ToggleTemplate;
    private List<Toggle> TogglesList;
    [SerializeField] private GameController GameControl;

    private void Start()
    {
        InitSceneList();
    }

    public void InitSceneList()
    {
        TogglesList = new List<Toggle>();
        ScenesList = GameControl.sceneNames;

        for (int i = 0; i < ScenesList.Length; i++)
        {
            GameObject go = Instantiate(ToggleTemplate, ScrollContent.transform);
            go.GetComponentInChildren<Text>().text = ScenesList[i];
            Toggle toggle = go.GetComponentInChildren<Toggle>();
            
            toggle.onValueChanged.AddListener((value) =>
            {
                OnListUpdate(i);
            });
            
            TogglesList.Add(toggle);
        }

        if (ToggleTemplate != null)
        {
            Destroy(ToggleTemplate);
            ToggleTemplate = null;
        }
    }

    public void OnListUpdate(int toggleIndex)
    {
        Debug.Log("La scene " + ScenesList[toggleIndex] +" n'est plus active");
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
