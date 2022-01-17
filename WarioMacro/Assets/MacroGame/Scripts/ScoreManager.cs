using System;
using System.Collections;
using System.Collections.Generic;
using GameTypes;
using TreeEditor;
using Unity.Mathematics;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{

    public int[] leaderboard = new int[10];
    
    public int score;
    
    public int moneyBag;
    
    [Header("3 MicroGames")] 
    [SerializeField]
    private int[] neutral = new int[4];
    [SerializeField]
    private int[] oneSpecialist= new int[4];
    [SerializeField]
    private int[] twoSpecialist = new int[4];
    
    [Header("3 MicroGames Alarm")] 
    [SerializeField]
    private int[] neutralAlarm = new int[4];
    [SerializeField]
    private int[] oneSpecialistAlarm = new int[4];
    [SerializeField]
    private int[] twoSpecialistAlarm = new int[4];

    [SerializeField] private int loseCharacter3SuccessCount = 0;
    
    [Header("5 MicroGames")] 
    [SerializeField]
    private int[] noSpecialist = new int[6];

    [Header("5 MicroGames Alarm")] 
    [SerializeField]
    private int[] noSpecialistAlarm = new int[6];

    [SerializeField] private int loseCharacter5SuccessCount = 1;
    
    
    void Start()
    {
        
        var scores = PlayerPrefs.GetString("leaderboard");
        if(scores == "") return;
        var scoresTab = scores.Split(';');
        for(int i = 0;i < 10 ;i++)
        {
            leaderboard[i] = int.Parse(scoresTab[i]);
        }
    }
    
    private void Update()
    {
        score = moneyBag * GameController.instance.characterManager.playerTeam.Count;
    }

    public void UpdateScore(int nodeSuccess,int gameCount,Stack<Character> team)
    {
        var type = GameController.instance.map.currentNode.GetComponent<NodeSettings>().type;
        switch (Alarm.isActive)
        {
            case false:
                if (gameCount == 3)
                {
                    if ( type == NodeType.None)
                        moneyBag += neutral[nodeSuccess];
                    if (CheckTeamTypes(team) == 1)
                        moneyBag += oneSpecialist[nodeSuccess];
                    if (CheckTeamTypes(team) > 1) 
                        moneyBag += twoSpecialist[nodeSuccess];
                }
                if (gameCount == 5)
                    moneyBag += noSpecialist[nodeSuccess];
                break;
            
            case true:
                if (gameCount == 3)
                {
                    if (type == NodeType.None)
                    {
                        moneyBag += neutralAlarm[nodeSuccess];
                        if(nodeSuccess<= loseCharacter3SuccessCount)
                            GameController.instance.characterManager.LoseCharacter();
                    }
                    if (CheckTeamTypes(team) == 1)
                        moneyBag += oneSpecialistAlarm[nodeSuccess];
                    if (CheckTeamTypes(team) > 1) 
                        moneyBag += twoSpecialistAlarm[nodeSuccess];
                }

                if (gameCount == 5)
                {
                    moneyBag += noSpecialistAlarm[nodeSuccess];
                    if(nodeSuccess<= loseCharacter5SuccessCount)
                        GameController.instance.characterManager.LoseCharacter();
                }
                break;
        }
    } 
    
    public int CheckTeamTypes(Stack<Character> team)
    {
        var count = 0;
        foreach (var character in team)
        {
            if (character.characterType == GameController.instance.map.currentNode.GetComponent<NodeSettings>().type)
            {
                count++;
            }
        }
        return count;
    }

    void UpdateLeaderboard()
    {
        if (score > leaderboard[9])
        {
            leaderboard[9] = score;
            Array.Reverse(leaderboard);
        }
        PlayerPrefs.DeleteAll();
        foreach (var score in leaderboard)
        {
            PlayerPrefs.SetString("leaderboard",score+";");
        }
    }
    
}
