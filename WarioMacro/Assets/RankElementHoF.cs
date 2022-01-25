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
        timeTMP.text = run.time.ToString();
        foreach (var key in run.team.Keys)
        {
            portraits[0].sprite = key.cardSprite;
            mask[0].SetActive(!run.team[key]);
            i++;
        }
    }
}
