using System;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

public class ScoreManager : MonoBehaviour
{
    [NonSerialized] public int currentMoney;
    public int currentRunMoney;
    [HideInInspector] public int scoreMultiplier = 1;
    
    [SerializeField] private PlayableDirector moneyBagsDirector;
    [SerializeField] private TextMeshProUGUI moneyBagsText;

    
    public void AddMoney(int addedValue)
    {
        currentRunMoney += addedValue * scoreMultiplier;
        moneyBagsText.text = currentRunMoney.ToString();
        moneyBagsDirector.Play();
        AudioManager.MacroPlaySound("CashGain", 0);
    }
    
    public void AddToCurrentMoney()
    {
        currentMoney += currentRunMoney;
        moneyBagsText.text = currentMoney.ToString();
        PlayerPrefs.SetInt("PlayerMoney", currentMoney);
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
