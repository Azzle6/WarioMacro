using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NAC1_MiniGameController : MonoBehaviour, ITickable
{
    [SerializeField] private NAC1_UIManager uiManager;
    //[SerializeField] private SoundManager soundManager;

   public AudioClip Vic;

    //[SerializeField] private int difficulty;
    //[SerializeField] private float tickTime;
    private int nbButtonPerSign;
    private int nbTickMax;

    private int winTick = -1;
    [SerializeField] private List<GameObject> signs;
    [SerializeField] private Sprite[] xboxButtonSprites = new Sprite[4];
    [SerializeField] private string[] inputs = new string[] { "A", "B", "X", "Y" };
    [SerializeField] private GameObject buttonPrefab;

    private GameObject button;
    public string[,] trueInputs;
    public int[] inputStreak;

    private SpriteRenderer buttonSpriteRenderer;
    private int rand;
    private int trueSign;

    private bool hasStarted;
    private bool hasEnded;
    private bool result;

    public GameObject body, def, end;
    private int tick;

    public void Start()
    {
        GameManager.Register();
        GameController.Init(this);

        hasStarted = false;

        SetDifficulty();

        InitializeArray();

        CreateSigns();
        uiManager.UpdateTimer(nbTickMax - GameController.currentTick);
        hasStarted = true;
        Debug.Log(result);

    }


    private void SetDifficulty()
    {
        Debug.Log(GameController.difficulty);
        switch (GameController.difficulty)
        {
            case 1:
                nbButtonPerSign = 3;
                nbTickMax = 7;
                break;
            case 2:
                nbButtonPerSign = 4;
                nbTickMax = 7;
                break;
            case 3:
                nbButtonPerSign = 5;
                nbTickMax = 7;
                break;
            default:
                nbButtonPerSign = 3;
                nbTickMax = 7;
                break;
        }
    }

    private void InitializeArray()
    {
        trueInputs = new string[signs.Count, nbButtonPerSign];
        inputStreak = new int[signs.Count];

        for (int i = 0; i < inputStreak.Length; i++)
        {
            inputStreak[i] = 0;
        }
    }

    private void CreateSigns()
    {
        trueSign = Random.Range(0, signs.Count);

        uiManager.InitializeTrueSign(trueSign);

        int s = 0;
        foreach (var sign in signs)
        {
            for (int i = 0; i < nbButtonPerSign; i++)
            {
                buttonSpriteRenderer = sign.transform.GetChild(i).GetComponent<SpriteRenderer>();
                rand = Random.Range(0, xboxButtonSprites.Length);
                buttonSpriteRenderer.sprite = xboxButtonSprites[rand];
                trueInputs[s, i] = inputs[rand];
            }
            s++;
        }
    }

    public void CheckInputsOrder(string input)
    {
        if (trueInputs[trueSign, inputStreak[trueSign]].Equals(input))
        {
            //soundManager.PlayShotSound();
            if (++inputStreak[trueSign] == nbButtonPerSign)
            {
                EndGame(true);
            }
        }
        else
        {
            EndGame(false);
        }
    }

    private void EndGame(bool result)
    {
        StopAllCoroutines();
        this.hasEnded = true;
        this.result = result;
        uiManager.EnableEndScreen(result);
    }

    private void DebugInputStreak()
    {
        Debug.Log("_________________");
        for (int i = 0; i < inputStreak.Length; i++)
        {
            Debug.Log("panneau " + i + " : " + inputStreak[i]);
        }
    }

    public int GetTrueSign()
    {
        return this.trueSign;
    }

    public bool HasStarted()
    {
        return this.hasStarted;
    }

    public int GetNbTick()
    {
        return GameController.currentTick;
    }

    public int GetNbTickMax()
    {
        return this.nbTickMax;
    }

    public bool GetHasEnded()
    {
        return hasEnded;
    }

    public void OnTick()
    {
        if (GameController.currentTick == winTick || GameController.currentTick == 8)
        {
            Debug.Log("Entered the last function");
            GameController.FinishGame(result);
        }
        Debug.Log(GameController.currentTick);
        Debug.Log("winTick : " + winTick);
        if (!hasEnded)
        {
            uiManager.UpdateTimer(nbTickMax - GameController.currentTick);
        }
        //if (GameController.currentTick == nbTickMax && !hasEnded) EndGame(false);


        if (hasEnded && winTick == -1)
        {
            Debug.Log("has ended before end");
            winTick = GameController.currentTick + 3;
            //   winTick = GameController.currentTick + 3;
            GameController.StopTimer();

        }



        /*if(uiManager.over == true && GameController.currentTick <=5)
        {
            winTick = 5;
          //  winTick = GameController.currentTick+5;
            GameController.StopTimer();
        }*/

        if (!hasEnded && GameController.currentTick == 5)
        {
            winTick = 8;
            if (result == false)
            {
                AudioManager.PlaySound(Vic, 1);
                body.SetActive(true);
                def.SetActive(true);
                end.SetActive(true);
            }
            GameController.StopTimer();
        }
        /*  if (GameController.currentTick == winTick)
          {
              GameController.FinishGame(result);
          }
        */
        /*if (GameController.currentTick == 8)
        {
            GameController.FinishGame(result);
        }*/


    }
}