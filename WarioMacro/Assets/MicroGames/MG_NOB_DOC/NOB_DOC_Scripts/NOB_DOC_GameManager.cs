using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NOB_DOC_GameManager : MonoBehaviour, ITickable
{
    public static NOB_DOC_GameManager instance;
    [SerializeField] private TMPro.TMP_Text tickText;
    [SerializeField] private int difficulty;
    [SerializeField] private List<GameObject> trolleys;
    [SerializeField] public bool resultPending;

    public void OnTick()
    {
        tickText.text = GameController.currentTick.ToString();
        if (GameController.currentTick == 5)
        {
            //Le jeu se finit, il nous reste 3 ticks pour afficher le résultat
        }

        if (GameController.currentTick == 8)
        {
            //Le jeu se décharge
            GameController.FinishGame(true);
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
    }
}
