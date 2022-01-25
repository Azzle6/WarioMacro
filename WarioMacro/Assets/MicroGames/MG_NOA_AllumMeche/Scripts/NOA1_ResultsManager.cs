using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NOA1_ResultsManager : MonoBehaviour, ITickable
{
    [SerializeField] private Animator camAnimator;
    [SerializeField] private Animator grilleAnimator;
    [SerializeField] private AudioClip[] sfx;
    private bool results;
    private bool isFinished;
    private int tickEnd = 10;
    private void Start()
    {
        GameManager.Register();
        GameController.Init(this);
    }
    
    public void OnTick()
    {
        if (GameController.currentTick == 5 && !isFinished)
        {
            if (results == false)
            {
                MiniGameLose();
                GameController.StopTimer();
            }
        }
        if ((isFinished) && tickEnd == 10) 
        {
            GameController.StopTimer();
            tickEnd = GameController.currentTick;
        }
        if (GameController.currentTick == tickEnd + 3) 
        {
            GameController.FinishGame(results);
        }
    }

    private void MiniGameLose()
    {
        AudioManager.PlaySound(sfx[1],1f);
        isFinished = true;
        camAnimator.SetBool("Go",true);
        StartCoroutine(WaitLoseAnim());
    }

    private IEnumerator WaitLoseAnim()
    {
        yield return new WaitForSeconds(0.6f);
        grilleAnimator.SetBool("Go",true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Match(Clone)")
        {
            if (other.GetComponent<NOA1_Matches>().isOnFire)
            {
                AudioManager.PlaySound(sfx[0],1f);
                AudioManager.PlaySound(sfx[2],1f,1.4f);
                isFinished = true;
                results = true;
                camAnimator.SetBool("Win",true);
            }
        }
    }
}
