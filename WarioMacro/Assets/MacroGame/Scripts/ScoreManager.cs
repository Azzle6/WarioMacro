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

    
    public void AddMoney(float addedValue)
    {
        currentRunMoney += (int) (addedValue * scoreMultiplier);
        ShowMoney();
        moneyBagsDirector.Play();
        AudioManager.MacroPlaySound("CashGain", 0);
    }

    public void ShowMoney()
    {
        moneyBagsText.text = currentRunMoney.ToString();
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
        
        currentMoney -= v;
        moneyBagsText.text = currentMoney.ToString();
        moneyBagsDirector.Play(); // Lose money animation ?
        PlayerPrefs.SetInt("PlayerMoney", currentMoney);
        AudioManager.MacroPlaySound("CashLose", 0);
        return true;
    }

    private void Awake()
    {
        currentMoney = PlayerPrefs.GetInt("PlayerMoney", 0);
        moneyBagsText.text = currentMoney.ToString();
    }
}
