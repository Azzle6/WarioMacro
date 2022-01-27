using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

public class EndScoreUI : MonoBehaviour
{
    public GameObject endPanel;
    public GameObject endSuccess;
    public GameObject endFailure;
    public Image[] defeatPortraits;
    public Image[] successPortraits;

    
    public IEnumerator ToggleEndSuccess()
    {
        var i = 0;
        foreach (var t in GameController.instance.hallOfFame.currentRun.team)
        {
            successPortraits[i].sprite = t.Key.portraitSprite;
            i++;
        }
        endPanel.SetActive(true);
        endSuccess.SetActive(true);
        AudioManager.MacroPlaySound("GameWin", 0);
        yield return new WaitForSeconds(6);
        CloseEndScore();
    }
    
    public IEnumerator ToggleEndFailure()
    {
        var i = 0;
        foreach (var t in GameController.instance.hallOfFame.currentRun.team)
        {
            defeatPortraits[i].sprite = t.Key.portraitSprite;
            i++;
        }
        endPanel.SetActive(true);
        endFailure.SetActive(true);
        AudioManager.MacroPlaySound("GameLose", 0);
        yield return new WaitForSeconds(6);
        CloseEndScore();
    }

    public void CloseEndScore()
    {
        endPanel.SetActive(false);
        endSuccess.SetActive(false);
        endFailure.SetActive(false);
    }
}
