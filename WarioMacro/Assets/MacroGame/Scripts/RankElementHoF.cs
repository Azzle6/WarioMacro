using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RankElementHoF : MonoBehaviour
{
    public TextMeshProUGUI scoreTMP;
    public TextMeshProUGUI timeTMP;
    public Image[] portraits = new Image[4];
    public GameObject[] mask = new GameObject[4];


    public void SetRank(HallOfFame.Run run)
    {
        var i = 0;
        scoreTMP.text = run.score.ToString() +"$";
        
        var minutes  = run.time / 60;
        var seconds  = run.time % 60;
        timeTMP.text = String.Format("{0:00}:{1:00}", minutes, seconds);
        foreach (var key in run.team.Keys)
        {
            portraits[i].sprite = key.lifebarSprite;
            mask[i].SetActive(!run.team[key]);
            i++;
        }
    }
}
