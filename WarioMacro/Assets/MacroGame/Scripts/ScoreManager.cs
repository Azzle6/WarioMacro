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

        moneyBagsText.text = moneyBag.ToString();
        moneyBagsDirector.Play();
        AudioManager.MacroPlaySound("CashGain", 0);
    }

    public bool Pay(int v)
    {
        if (v > moneyBag) return false;

        moneyBag -= v;
        moneyBagsText.text = moneyBag.ToString();
        moneyBagsDirector.Play(); // Lose money animation ?
        AudioManager.MacroPlaySound("CashLose", 0);
        return true;
    }
}
