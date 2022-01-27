using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndScoreUI : MonoBehaviour
{
    public GameObject endPanel;
    public GameObject endSuccess;
    public GameObject endFailure;
    public Image[] defeatPortraits;
    public Image[] successPortraits;
    public TextMeshProUGUI score;
    public TextMeshProUGUI nbMicroGames;
    public TextMeshProUGUI nbSpecialistMicroGames;


    public void ToggleEndSuccess()
    {
        var i = 0;
        foreach (var t in GameController.instance.hallOfFame.currentRun.team)
        {
            successPortraits[i].sprite = t.Key.portraitSprite;
            i++;
        }
        score.text = GameController.instance.hallOfFame.currentRun.score +"$";
        nbMicroGames.text = "Micro Games Played : " + GameController.instance.nbMicroGames;
        nbSpecialistMicroGames.text = "Specialist Games Played : "+GameController.instance.nbMicroGamesSpecialist;
        
        endPanel.SetActive(true);
        endSuccess.SetActive(true);
    }
    
    public void ToggleEndFailure()
    {
        var i = 0;
        foreach (var t in GameController.instance.hallOfFame.currentRun.team)
        {
            defeatPortraits[i].sprite = t.Key.portraitSprite;
            i++;
        }
        endPanel.SetActive(true);
        endFailure.SetActive(true);
    }

    public void CloseEndScore()
    {
        endPanel.SetActive(false);
        endSuccess.SetActive(false);
        endFailure.SetActive(false);
    }
}
