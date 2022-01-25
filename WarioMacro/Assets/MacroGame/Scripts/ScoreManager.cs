using System;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

public class ScoreManager : MonoBehaviour
{
    [NonSerialized] public int currentMoney;
    [HideInInspector] public int scoreMultiplier = 1;
    
    [SerializeField] private Leaderboard leaderBoard;
    [SerializeField] private PlayableDirector moneyBagsDirector;
    [SerializeField] private TextMeshProUGUI moneyBagsText;

    

    public void FinalScore()
    {
        leaderBoard.UpdateLeaderboard(currentMoney * GameController.instance.characterManager.playerTeam.Count);
    }

    public void AddMoney(int addedValue)
    {
        currentMoney += addedValue * scoreMultiplier;

        PlayerPrefs.SetInt("PlayerMoney", currentMoney);
        moneyBagsText.text = currentMoney.ToString();
        moneyBagsDirector.Play();
        AudioManager.MacroPlaySound("CashGain", 0);
    }

    public bool Pay(int v)
    {
        if (v > currentMoney) return false;

        PlayerPrefs.SetInt("PlayerMoney", currentMoney);
        currentMoney -= v;
        moneyBagsText.text = currentMoney.ToString();
        moneyBagsDirector.Play(); // Lose money animation ?
        AudioManager.MacroPlaySound("CashLose", 0);
        return true;
    }

    private void Awake()
    {
        currentMoney = PlayerPrefs.GetInt("PlayerMoney", 0);
        moneyBagsText.text = currentMoney.ToString();
    }
}
