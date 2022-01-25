using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NAC3_EnRappelController : MonoBehaviour, ITickable
{

    [SerializeField] private GameObject[] easyLayouts;
    [SerializeField] private GameObject[] mediumLayouts;
    [SerializeField] private GameObject[] hardLayouts;
    [SerializeField] private GameObject victoryCanvas;
    [SerializeField] private GameObject defeatCanvas;

    public bool hasEnded;
    private bool result;
    private int tickEnd;

    private void Awake()
    {
        tickEnd = -1;
        hasEnded = false;
    }

    private void Start()
    {
        GameManager.Register();
        GameController.Init(this);
        SetDifficulty();
    }

    private void SetDifficulty() 
    {
        switch (GameController.difficulty) 
        {
            case 1:
                easyLayouts[Random.Range(0, easyLayouts.Length)].SetActive(true);
                break;
            case 2:
                mediumLayouts[Random.Range(0, mediumLayouts.Length)].SetActive(true);
                break;
            case 3:
                hardLayouts[Random.Range(0, hardLayouts.Length)].SetActive(true);
                break;
        }
    }

    public void CheckEndGame(bool result) 
    {
        hasEnded = true;
        if (result)
        {
            this.result = true;
            Victory();
        }
        else 
        {
            this.result = false;
            Defeat();
        }
    }

    private void Victory() 
    {
        victoryCanvas.SetActive(true);
    }

    private void Defeat() 
    {
        defeatCanvas.SetActive(true);
    }


    public void OnTick()
    {
        Debug.Log("Tick end" + tickEnd);
        Debug.Log("Tick" + GameController.currentTick);
        
        if (hasEnded && tickEnd == -1) 
        {
            GameController.StopTimer();
            tickEnd = GameController.currentTick + 3;
        }
        
        if (GameController.currentTick == 5 && !hasEnded) 
        {
            tickEnd = GameController.currentTick + 3;
            GameController.StopTimer();
            CheckEndGame(false);
        }
        
        if (GameController.currentTick == tickEnd) 
        {
            Debug.Log("Result : " + result);
            GameController.FinishGame(true);
        }
        
    }

    public bool getHasEnded() 
    {
        return hasEnded;
    }

}
