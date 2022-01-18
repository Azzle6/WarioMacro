using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{
    [SerializeField] private int[] leaderboard = new int[10];
    

    void Start()
    {
        var scores = PlayerPrefs.GetString("leaderboard");
        if (scores == "") return;
        var scoresTab = scores.Split(';');
        leaderboard = new int[Mathf.Clamp(scoresTab.Length,0,10)];
        for (int i = 0; i < scoresTab.Length; i++)
        {
            var value = 0;
            var tried = int.TryParse(scoresTab[i], out value);
            if (tried) leaderboard[i] = value;
        }
    }

    private void ShowLeaderboard()
    {
        var scores = PlayerPrefs.GetString("leaderboard");
        Debug.Log(scores);
    }
    
    public void UpdateLeaderboard(int score)
    {
        var lb = "";
        if (leaderboard.Length == 10 && score < leaderboard[leaderboard.Length-1]) return;
        for (int i = 0; i < leaderboard.Length; i++) if (leaderboard[i] == score) return;
        if (leaderboard.Length == 10) leaderboard[9] = score;
        if (leaderboard.Length < 10) leaderboard[leaderboard.Length-1] = score;
        Array.Sort(leaderboard);
        Array.Reverse(leaderboard);
        PlayerPrefs.DeleteAll();
        for (int i = 0; i < leaderboard.Length; i++)
        {
            if(leaderboard[i] != 0)
                lb += leaderboard[i] + ";";
        }
        PlayerPrefs.SetString("leaderboard", lb);
        ShowLeaderboard();
    }
    
    
}

