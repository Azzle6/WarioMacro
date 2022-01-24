using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NAB2_GameManager : MonoBehaviour, ITickable
{
    public GameConfig gameController;
    public NAB2_Shot shotScript;
    public NAB2_TICCounter ticCounter;

    public bool gameIsOver = false;

    public bool failure = false, success = false;

    public NAB2_FailureAnimation failureAnim;
    public GameObject failedAnim, successAnim;

    public int ticWait = 8;
    public bool earlyAction = false;


    void Start()
    {
        GameManager.Register();
        GameController.Init(this);
    }


    public void OnTick()
    {

        if (shotScript.win == true && earlyAction == false)
        {
            successAnim.SetActive(true);
            ticWait = 3;
            earlyAction = true;
            GameController.StopTimer();
        }

        if (shotScript.win == false && earlyAction == false && shotScript.shotFired == true)
        {
            failedAnim.SetActive(true);
            ticWait = 3;
            earlyAction = true;
            GameController.StopTimer();
        }

        FinishAnim();
        //ticCounter.TicAvance();
        if(GameController.currentTick == 5)
        {
            GameController.StopTimer();
            gameIsOver = true;
            if(shotScript.win == false)
            {
                //failureAnim.Animation();
                failedAnim.SetActive(true);
                
            }
            else if(shotScript.win == true)
            {
                successAnim.SetActive(true);

            }
        }







    }

    void FinishAnim()
    {
        ticWait--;
        if (ticWait == -1)
        {
            GameController.FinishGame(true);
        }

    }
}
