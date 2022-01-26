using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ULA2_GameManager : MonoBehaviour, ITickable
{
    public GameObject[] fishList;

    public ULA2_ArmScript armScript;

    public GameControllerSO gameController;

    public GameObject victoryImage;
    public GameObject defeatImage;

    int tick;
    private void Awake()
    {
        fishList[gameController.currentDifficulty-1].SetActive(true);
        armScript.fishOffsetX = fishList[gameController.currentDifficulty-1].GetComponent<ULA2_FishInfo>().fishOffset;
        armScript.maxScore = fishList[gameController.currentDifficulty-1].GetComponent<ULA2_FishInfo>().fishMaxScore;
        armScript.fish = fishList[gameController.currentDifficulty-1];
    }

    private void Start()
    {
        GameManager.Register();
        GameController.Init(this);
    }

    public void OnTick()
    {
        if (!armScript.gameOver)
        {
            tick = GameController.currentTick + 3;

        }

        

        if (armScript.counter >= armScript.maxScore)
        {
            victoryImage.SetActive(true);
            armScript.gameOver = true;
            armScript.gameResult = true;
            Debug.Log("Game Result : " + armScript.gameResult);
        }
        else if(GameController.currentTick == 5 && armScript.counter >= armScript.maxScore)
        {
            victoryImage.SetActive(true);
            armScript.gameOver = true;
            Debug.Log("Game Result : " + armScript.gameResult);
        }
        else if (GameController.currentTick == 5 && armScript.counter < armScript.maxScore)
        {
            defeatImage.SetActive(true);
            armScript.gameOver = true;
            Debug.Log("Game Result : " + armScript.gameResult);
        }

        if(GameController.currentTick == 5)
        {
            armScript.canPressButton = false;
            armScript.gameOver = true;
        }

        if(armScript.gameOver)
        {
            GameController.StopTimer();

        }

        if (GameController.currentTick >= tick)
        {
            Debug.Log("RESTART GAME");
            GameController.FinishGame(armScript.gameResult);
        }

        //Debug.LogError(tick);
        //Debug.Log(GameController.currentTick);
    }
}
