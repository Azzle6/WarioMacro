using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NAC2_QuelleCouleurController : MonoBehaviour, ITickable
{
    [SerializeField] private int difficulty;
    private int nbBalls;
    private int tickSecondPhase; //Le tick durant lequel est affiché le menu de sélection des réponses
    [SerializeField] private NAC2_MoveBall[] balls;
    //[SerializeField] private Color[] colors;
    [SerializeField] private Sprite[] sprites;

    [SerializeField] private NAC2_AnswerManager answerManager;
    [SerializeField] private GameObject answerMenu;
    [SerializeField] private GameObject defeat, victory;

    private int[] goodColors;
    private bool result;
    private bool hasEnded;
    private int goodAnswer;
    public AudioClip audioClip, audioClip2, audioClip3;
    private int endTick = -1;
    bool noNound = false;

    private void Awake()
    {
        GameManager.Register();
        GameController.Init(this);
        GameController.Register();
        balls[0].SetYOffset(Random.Range(0f, 10f));


        goodColors = new int[] { -1, -1 };
    }

    private int rand;

    private void Start()
    {
        hasEnded = false;
        SetDifficulty();
        rand = -1;
        Debug.Log(result);
    }

    private void SetDifficulty()
    {
        switch (GameController.difficulty)
        {
            case 1:
                nbBalls = 1;
                break;
            case 2:
                nbBalls = 2;
                break;
            case 3:
                nbBalls = 2;
                break;
            default:
                nbBalls = 1;
                break;
        }
    }
    public void OnTick()
    {
        Debug.Log(GameController.currentTick);
        Debug.Log("end Tick : " + endTick);
        Debug.Log("HasEnded : " + hasEnded);
        if (hasEnded && endTick == -1)
        {
            Debug.Log("3 last ticks");
            endTick = GameController.currentTick + 3;
            GameController.StopTimer();
        }
        if (GameController.currentTick == 1)
        {
            LaunchBall(0);
            if (GameController.difficulty > 1)
                LaunchBall(1);
        }
        if (GameController.currentTick == 2)
        {
            answerManager.GenerateAnswers(goodColors);
            answerMenu.SetActive(true);

        }
        if (GameController.currentTick == 3)
        {
           // answerMenu.SetActive(true);
        }

        if (GameController.currentTick == 5)
        {
            if (result == false && noNound == false)
            {
                answerMenu.SetActive(false);
                defeat.SetActive(true);
                GameController.StopTimer();
                AudioManager.PlaySound(audioClip2, 1);
            }
        }





        if (GameController.currentTick == 8 || GameController.currentTick == endTick)
        {
            GameController.FinishGame(result);
            print("fin");

        }
        /*   if (GameController.currentTick == 8) 
           {
               GameController.FinishGame(result);
               print("8huit");
           } */
    }

    private int newRand;

    private void LaunchBall(int idx)
    {
        AudioManager.PlaySound(audioClip, 1);
        balls[idx].SetCanMove(true);
        RandomColorIndex(idx);
        goodColors[idx] = rand;
        balls[idx].GetComponent<SpriteRenderer>().sprite = sprites[rand];
    }

    private void RandomColorIndex(int idx)
    {
        do
        {
            newRand = Random.Range(0, sprites.Length - idx);
        } while (rand == newRand);
        rand = newRand;
    }

    public void CheckAwnser(int awnser)
    {
        hasEnded = true;
        goodAnswer = answerManager.GetGoodAnswer();
        if (awnser == goodAnswer)
        {
            //  GameController.FinishGame(true);
            result = true;
            Debug.Log(result);
            answerMenu.SetActive(false);
            victory.SetActive(true);
            AudioManager.PlaySound(audioClip3, 1);
            Debug.Log("TRUE");
        }
        else
        {
            //GameController.FinishGame(false);
            result = false;
            noNound = true;
            Debug.Log(result);
            Debug.Log("FALSE");
            answerMenu.SetActive(false);
            defeat.SetActive(true);
               AudioManager.PlaySound(audioClip2, 1);
        }
    }
}