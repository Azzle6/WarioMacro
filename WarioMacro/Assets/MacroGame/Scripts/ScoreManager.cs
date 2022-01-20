using System.Collections.Generic;
using GameTypes;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    
    [SerializeField] private Leaderboard leaderBoard;
    
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

    [Header("5 MicroGames")] 
    [SerializeField]
    private int[] noSpecialist = new int[6];

    [Header("5 MicroGames Alarm")] 
    [SerializeField]
    private int[] noSpecialistAlarm = new int[6];

    private void FinalScore()
    {
        score = moneyBag * GameController.instance.characterManager.playerTeam.Count;
        leaderBoard.UpdateLeaderboard(score);
    }

    public void UpdateScore(int nodeSuccess,int gameCount,Stack<Character> team)
    {
        var type = GameController.instance.map.currentNode.GetComponent<NodeSettings>().type;
        int bagsBeforeUpdate = moneyBag;
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
                    }
                    if (CheckTeamTypes(team) == 1)
                        moneyBag += oneSpecialistAlarm[nodeSuccess];
                    if (CheckTeamTypes(team) > 1) 
                        moneyBag += twoSpecialistAlarm[nodeSuccess];
                }

                if (gameCount == 5)
                {
                    moneyBag += noSpecialistAlarm[nodeSuccess];
                }
                break;
        }

        AudioManager.MacroPlaySound(moneyBag > bagsBeforeUpdate ? "CashGain" : "CashLose", 0);
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
    
    
}
