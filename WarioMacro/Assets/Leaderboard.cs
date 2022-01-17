using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{
    public string[] leaderboard = new string[10];
    
    public int score;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UpdateLeaderboard();
            ShowLeaderboards();
        }
    }

    void Start()
    {
        
        var scores = PlayerPrefs.GetString("leaderboard");
        if(scores == "") return;
        var scoresTab = scores.Split(';');
        leaderboard = new string[scoresTab.Length];
        for(int i = 0;i < scoresTab.Length ;i++)
        {
            leaderboard[i] = scoresTab[i];
        }
        Debug.Log(scores);
    }

    void ShowLeaderboards()
    {
        var scores = PlayerPrefs.GetString("leaderboard");
        Debug.Log(scores);
    }
    

    void UpdateLeaderboard()
    {
        var lb = "";
        var min = 0;
        var full = int.TryParse(leaderboard[9],out min);
        if(full)
            if (score < int.Parse(leaderboard[9])) return;
        leaderboard[9] = score.ToString();
        Array.Reverse(leaderboard);
        PlayerPrefs.DeleteAll();
        for (int i = 0; i < 10; i++)
        {
            if (leaderboard[i] != "")
                lb += leaderboard[i] + ";";

        }
        PlayerPrefs.SetString("leaderboard", lb);
    }

    string[] GetLeaderboard()
    {
        var scores = PlayerPrefs.GetString("leaderboard");
        if(scores == "") return null;
        var scoresTab = scores.Split(';');
        for(int i = 0;i < 10 ;i++)
        {
            leaderboard[i] = scoresTab[i];
        }
        return scoresTab;
    }
    
}

