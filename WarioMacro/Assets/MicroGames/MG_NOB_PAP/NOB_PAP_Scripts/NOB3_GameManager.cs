using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NOB3_GameManager : MonoBehaviour, ITickable
{
    [SerializeField] private NOB3_PlankManager plankManager;
    [SerializeField] private GameObject crowbar;
    [SerializeField] private TMPro.TMP_Text tickCountText;
    [SerializeField] private GameObject victoryText, defeatText;
    public bool result;
    public bool gameEnded;
    private int endTick = 10;
    private int tickCount;
    void Awake()
    {
        tickCountText.text = "0";
        GameManager.Register();
        GameController.Init(this);
    }
    public void OnTick()
    {
        tickCountText.text = GameController.currentTick.ToString();
        
        if ((GameController.currentTick == 5 || plankManager.planksLeft == 0) && !gameEnded)
        {
            crowbar.SetActive(false);
            endTick = GameController.currentTick;
            gameEnded = true;
            GameController.StopTimer();
            if (plankManager.planksLeft == 0)
            {
                result = true;
                victoryText.SetActive(true);
            }
            else
            {
                result = false;
                defeatText.SetActive(true);
            }
        }

        if (GameController.currentTick == endTick + 3)
        {
            GameController.FinishGame(result);
        }
    }
}
