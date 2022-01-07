using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;
using Random = UnityEngine.Random;

public class NOB_DOC_GameManager : MonoBehaviour, ITickable
{
    public static NOB_DOC_GameManager instance;
    [SerializeField] private TMPro.TMP_Text resultText;
    private bool result;
    [SerializeField] private TMPro.TMP_Text tickText;
    [SerializeField] private int difficulty;
    [SerializeField] private List<GameObject> trolleys;
    [SerializeField] public bool resultPending;
    private int stopTick;

    public void OnTick()
    {
        tickText.text = GameController.currentTick.ToString();

        if (GameController.currentTick == stopTick)
        {
            if (difficulty == 2)
            {
                trolleys[0].GetComponent<NOB_DOC_TrolleyBehaviour>().stopped = true;
            }
            else if (difficulty == 3)
            {
                trolleys[1].GetComponent<NOB_DOC_TrolleyBehaviour>().stopped = true;
            }
        }
        
        if (GameController.currentTick == stopTick + 1)
        {
            if (difficulty == 2)
            {
                trolleys[0].GetComponent<NOB_DOC_TrolleyBehaviour>().stopped = false;
            }
            else if (difficulty == 3)
            {
                trolleys[1].GetComponent<NOB_DOC_TrolleyBehaviour>().stopped = false;
            }
        }
        
        if (GameController.currentTick == 5)
        {
            
            if (result)
            {
                resultText.text = "Victory";
            }
            else
            {
                resultText.text = "Defeat";
            }
            resultText.gameObject.SetActive(true);
            //Le jeu se finit, il nous reste 3 ticks pour afficher le résultat
        }

        if (GameController.currentTick == 8)
        {
            //Le jeu se décharge
            GameController.FinishGame(result);
        }
    }
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        GameManager.Register(); //Mise en place du Input Manager, du Sound Manager et du Game Controller
        GameController.Init(this); //Permet a ce script d'utiliser le OnTick
        difficulty = GameController.difficulty;
        switch (difficulty)
        {
            case 1:
                trolleys[0].SetActive(true);
                break;
            case 2:
                trolleys[0].SetActive(true);
                trolleys[0].GetComponent<NOB_DOC_TrolleyBehaviour>().shouldStop = true;
                break;
            case 3:
                trolleys[1].SetActive(true);
                trolleys[1].GetComponent<NOB_DOC_TrolleyBehaviour>().shouldStop = true;
                break;
            default:
                trolleys[0].SetActive(true);
                break;
        }
        stopTick = Random.Range(2, 4);
    }

    public void SetResult(bool newResult)
    {
        if (resultPending)
        {
            resultPending = false;
            result = newResult;
            
            Debug.Log("Mini-game result : " + result);
        }
    }
}
