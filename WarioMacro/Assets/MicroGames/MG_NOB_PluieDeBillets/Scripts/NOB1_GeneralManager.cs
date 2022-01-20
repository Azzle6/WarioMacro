using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NOB1_GeneralManager : MonoBehaviour, ITickable
{
    bool result = true;

    int finalLives;

    public GameObject keyword;

    public GameObject victory;
    public GameObject defeat;

    public AudioClip defeatSound;
    public AudioClip victorySound;

    public GameObject firstMisstake;

    int stopTimerTick;
    bool timeStopped = false;
    bool gameFinished = false;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Register();
        GameController.Init(this);

        if (GameController.difficulty == 1)
        {
            PlayerPrefs.SetInt("Lives", 3);
        }
        else if (GameController.difficulty == 2)
        {
            PlayerPrefs.SetInt("Lives", 2);
        } 
        else if (GameController.difficulty == 3)
        {
            PlayerPrefs.SetInt("Lives", 1);
            firstMisstake.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTick()
    {
        //Suppression Keyword
        if (GameController.currentTick == 1)
        {
            keyword.SetActive(false);
        }

        finalLives = PlayerPrefs.GetInt("Lives");
        
        if (finalLives <= 0 & timeStopped == false & gameFinished == false)
        {
            result = false;
            GameController.StopTimer();
            stopTimerTick = GameController.currentTick;
            defeat.SetActive(true);
            AudioManager.PlaySound(defeatSound);
            timeStopped = true;
        }
        else if (timeStopped == true & GameController.currentTick == stopTimerTick+3)
        {
            GameController.FinishGame(result);
        }
        else if (GameController.currentTick == 5)
        {
            result = true;
            gameFinished = true;
            GameController.StopTimer();
            victory.SetActive(true);
            AudioManager.PlaySound(victorySound);
        }
        else if (GameController.currentTick == 8)
        {
            GameController.FinishGame(result);
        }
    }

}
