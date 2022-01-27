using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HallOfFameUI : MonoBehaviour
{
    
    
    public GameObject hallPanel;
    public RankElementHoF[] ranks = new RankElementHoF[10]; 
    public ScrollRect scrollRect;
    public bool isOpen;
    public float scrollSensitivity = 0.1f;

    private void Update()
    {
        if (!isOpen) return;
        InputManager.GetAxis(ControllerAxis.LEFT_STICK_VERTICAL, true);
        scrollRect.verticalNormalizedPosition += InputManager.GetAxis(ControllerAxis.LEFT_STICK_VERTICAL,true)*scrollSensitivity;
        if(InputManager.GetKeyDown(ControllerKey.B, true) && isOpen) CloseHall();
    }
    public void OpenHall()
    {
        hallPanel.SetActive(true);
        isOpen = true;
        InputManager.lockInput = true;
        setRanksUI();
    }

    
    public void setRanksUI()
    {
        var i = 0;
        foreach (var rank in GameController.instance.hallOfFame.hall)
        {
            ranks[i].gameObject.SetActive(true);
            ranks[i].SetRank(rank);
            i++;
        }
    }
    public void CloseHall()
    {
        hallPanel.SetActive(false);
        isOpen = false;
        InputManager.lockInput = false;
        GameController.OnInteractionEnd();
        AudioManager.MacroPlayRandomSound("BarmanExit");
    }
}
