using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlaytestTool : MonoBehaviour
{
    [SerializeField] private string[] ScenesList = null;
    [SerializeField] private GameObject ScrollContent;
    [SerializeField] private GameObject ToggleTemplate;
    [SerializeField] private GameObject playTestPanel;
    [SerializeField] private GameController GameControl;
    [SerializeField] private ScenesReferencesSO ScenesRefs;
    [SerializeField] private RecruitmentController RecruitControl;
    [SerializeField] private MapManager mapManager;
    [SerializeField] private TMP_Text MiniGameInfoText;
    [SerializeField] private TextMeshProUGUI mapNameText;
    private List<Toggle> TogglesList;
    private bool panelIsActive;

    private void Start()
    {
        InitSceneList();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            panelIsActive = !panelIsActive;
            playTestPanel.SetActive(panelIsActive);
        }

        if (GameController.instance.currentScene != null)
        {
            string sceneName = GameController.instance.currentScene;
            if(sceneName.Length > 0 )MiniGameInfoText.text = "Current MiniGame : " + sceneName.Substring(13, sceneName.Length - 13);
        }
        
        if (mapManager.currentMapGO != null)
        {
            string mapName = mapManager.currentMapGO.name;
            if(mapName.Length > 0 ) mapNameText.text = "Current Level : " + mapName.Substring(0, mapName.Length - 7);
        }
    }

    public void InitSceneList()
    {
        TogglesList = new List<Toggle>();

        List<String> SNames = new List<string>();
        foreach (MiniGameScriptableObject MGSO in ScenesRefs.MiniGames)
        {
            SNames.Add(MGSO.MiniGameSceneName);
        }

        GameControl.sceneNames = SNames.ToArray();
            
        ScenesList = GameControl.sceneNames;

        for (int i = 0; i < ScenesList.Length; i++)
        {
            GameObject go = Instantiate(ToggleTemplate, ScrollContent.transform);
            go.GetComponentInChildren<TMP_Text>().text = ScenesList[i].Substring(13, ScenesList[i].Length - 13);

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
        var SNames = new List<string>();
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
    
    public void SelectAll()
    {
        foreach (Toggle tog in TogglesList)
        {
            tog.isOn = !tog.isOn;
        }
    }

    public void SkipRecruitPhase()
    {
        RecruitControl.SkipRecruitment();
    }

    public void ResetPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
}
