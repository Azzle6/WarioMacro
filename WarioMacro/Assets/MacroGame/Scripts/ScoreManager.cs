using System;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

public class ScoreManager : MonoBehaviour
{
    [HideInInspector] public int scoreMultiplier = 1;
    
    [SerializeField] private Leaderboard leaderBoard;
    [SerializeField] private PlayableDirector moneyBagsDirector;
    [SerializeField] private TextMeshProUGUI moneyBagsText;

    private int moneyBag;

    public void FinalScore()
    {
        leaderBoard.UpdateLeaderboard(moneyBag * GameController.instance.characterManager.playerTeam.Count);
    }

    public void AddMoney(int addedValue)
    {
        moneyBag += addedValue * scoreMultiplier;

        PlayerPrefs.SetInt("PlayerMoney", moneyBag);
        moneyBagsText.text = moneyBag.ToString();
        moneyBagsDirector.Play();
        AudioManager.MacroPlaySound("CashGain", 0);
    }

    public bool Pay(int v)
    {
        if (v > moneyBag) return false;

        PlayerPrefs.SetInt("PlayerMoney", moneyBag);
        moneyBag -= v;
        moneyBagsText.text = moneyBag.ToString();
        moneyBagsDirector.Play(); // Lose money animation ?
        AudioManager.MacroPlaySound("CashLose", 0);
        return true;
    }

    private void Awake()
    {
        moneyBag = PlayerPrefs.GetInt("PlayerMoney", 0);
        moneyBagsText.text = moneyBag.ToString();
    }
}
