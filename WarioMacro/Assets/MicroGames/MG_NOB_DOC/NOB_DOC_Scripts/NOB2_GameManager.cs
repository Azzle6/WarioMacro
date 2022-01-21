using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;
using Random = UnityEngine.Random;

public class NOB2_GameManager : MonoBehaviour, ITickable
{
    public static NOB2_GameManager instance;
    [SerializeField] private GameObject victoryText, defeatText;
    [SerializeField] private AudioClip victoryAudio, defeatAudio;
    private bool result;
    [SerializeField] private TMPro.TMP_Text tickText;
    [SerializeField] private int difficulty;
    [SerializeField] private List<GameObject> trolleys;
    [SerializeField] public bool resultPending;
    private int endTick = 10;
    private bool gameEnded = false;
    private int stopTick;

    public void OnTick()
    {
        Debug.Log(endTick+3);
        tickText.text = GameController.currentTick.ToString();
        if (GameController.currentTick == stopTick)
        {
            if (difficulty == 2)
            {
                trolleys[0].GetComponent<NOB2_TrolleyBehaviour>().stopped = true;
            }
            else if (difficulty == 3)
            {
                trolleys[1].GetComponent<NOB2_TrolleyBehaviour>().stopped = true;
            }
        }
        
        if (GameController.currentTick == stopTick + 1)
        {
            if (difficulty == 2)
            {
                trolleys[0].GetComponent<NOB2_TrolleyBehaviour>().stopped = false;
            }
            else if (difficulty == 3)
            {
                trolleys[1].GetComponent<NOB2_TrolleyBehaviour>().stopped = false;
            }
        }
        
        if ((GameController.currentTick == 5 || !resultPending) && !gameEnded)
        {
            GameController.StopTimer();
            if (result)
            {
                victoryText.SetActive(true);
                AudioManager.PlaySound(victoryAudio);
            }
            else
            {
                defeatText.SetActive(true);
                AudioManager.PlaySound(defeatAudio);
            }
            gameEnded = true;
            endTick = GameController.currentTick;
        }

        if (GameController.currentTick == endTick + 3)
        {
            GameController.FinishGame(result);
        }
    }
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        GameManager.Register();
        GameController.Init(this);
        difficulty = GameController.difficulty;
        switch (difficulty)
        {
            case 1:
                trolleys[0].SetActive(true);
                break;
            case 2:
                trolleys[0].SetActive(true);
                trolleys[0].GetComponent<NOB2_TrolleyBehaviour>().shouldStop = true;
                break;
            case 3:
                trolleys[1].SetActive(true);
                trolleys[1].GetComponent<NOB2_TrolleyBehaviour>().shouldStop = true;
                break;
            default:
                trolleys[0].SetActive(true);
                break;
        }
        stopTick = 2/*Random.Range(2, 4)*/;
    }

    public void SetResult(bool newResult)
    {
        if (resultPending)
        {
            resultPending = false;
            result = newResult;

            if (!result)
            {
                NOB2_CageBehaviour.instance.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                NOB2_CageBehaviour.instance.Fall(new Vector2(NOB2_PlayerMovement.instance.transform.position.x, NOB2_PlayerMovement.instance.transform.position.y));
            }

            Debug.Log("Mini-game result : " + result);
        }
    }
}
