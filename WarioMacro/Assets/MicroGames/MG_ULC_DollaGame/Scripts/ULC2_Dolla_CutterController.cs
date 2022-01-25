using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ULC2_Dolla_CutterController : MonoBehaviour, ITickable
{
    private Vector2 joystickInput;
    [SerializeField] private GameObject correctImmage, victoryScreen, failScreen, cutFeedbackObj;
    [SerializeField] private float speed = 5f;
    public bool canCut, hasFinishedGame, success, canDeleteCorners = false;
    public int cornersReached = 0, billsToCut;
    [SerializeField] private List<GameObject> dollaBills;
    private AudioSource audioSource;
    [SerializeField] private AudioClip cutSound, katching, mmmWhatchuSay;
    private bool canStop = false;

    private void Start()
    {
        //Intégration au Macro Jeu
        GameManager.Register(); //Mise en place du Input Manager, du Sound Manager et du Game Controller
        GameController.Init(this); //Permet a ce script d'utiliser le OnTick

        audioSource = GetComponent<AudioSource>();
        hasFinishedGame = false;
        
        if (GameController. difficulty == 1)
        {
            billsToCut = 1;
            ActivateBills(1);
        }
        else if (GameController. difficulty == 2)
        {
            billsToCut = 2;
            ActivateBills(2);
        }
        else if(GameController. difficulty == 3)
        {
            billsToCut = 2;
            speed = speed * 1.5f;
            ActivateBills(2);
        }
    }

    void Update()
    {
        if (!hasFinishedGame)
        {
            //Joystick Input
            joystickInput = new Vector2(InputManager.GetAxis(ControllerAxis.LEFT_STICK_HORIZONTAL),
                -InputManager.GetAxis(ControllerAxis.LEFT_STICK_VERTICAL));
            transform.position += new Vector3(joystickInput.x, -joystickInput.y, 0) * speed * GameController.gameBPM/60;

            if (InputManager.GetKeyDown(ControllerKey.A) && canCut)
            {
                Debug.Log("Player Has Cut a Bill");
                billsToCut--;
                Instantiate(cutFeedbackObj, transform.position, Quaternion.identity);
                if (billsToCut > 0)
                {
                    audioSource.Play();
                    canDeleteCorners = true;
                }
            }
            else if (InputManager.GetKeyDown(ControllerKey.A) && !canCut)
            {
                canStop = true;
                hasFinishedGame = true;
                success = false;
                failScreen.SetActive(true);
                StartCoroutine(HolupALilBit(2.7f));
            }
        }

        //Bill Cut Check
        if (cornersReached == 4)
        {
            canCut = true;
            correctImmage.SetActive(true);
        }
        else
        {
            canCut = false;
            correctImmage.SetActive(false);
        }

        if (billsToCut == 0 && !hasFinishedGame)
        {
            canStop = true;
            hasFinishedGame = true;
            success = true;
            victoryScreen.SetActive(true);
            StartCoroutine(HolupALilBit(2f));
        }
    }

    void ActivateBills(int numberOfBills)
    {
        for (int i = numberOfBills; i > 0; i--)
        {
            int rng = Random.Range(0, dollaBills.Count);
            dollaBills[rng].SetActive(true);
            dollaBills.Remove(dollaBills[rng]);
        }
    }
    
    IEnumerator HolupALilBit(float yieldTime)
    {
        yield return new WaitForSeconds(yieldTime);
        GameController.FinishGame(success);
    }

    public void OnTick()
    {
        if (GameController.currentTick == 5 && !hasFinishedGame)
        {
            canStop = true;
            hasFinishedGame = true;
            success = false;
            failScreen.SetActive(true);
        }

        if (canStop)
        {
            GameController.StopTimer();
            canStop = false;
        }

        if (GameController.currentTick == 8)
        {
            GameController.FinishGame(success);
        }
    }
}
