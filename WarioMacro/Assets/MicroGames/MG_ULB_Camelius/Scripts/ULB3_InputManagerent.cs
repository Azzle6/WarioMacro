using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ULB3_InputManagerent : MonoBehaviour, ITickable
{
    bool AxisUpConfirmed = false;
    bool AxisDownConfirmed = false;
    int pointToWin;
    int point ;

    bool result;

    public Animator leftBar;
    public Animator rightBar;
    public Animator BrasYov;

    [SerializeField]
    Slider slider;

    [SerializeField]
    AudioClip TavernSound;
    [SerializeField]
    AudioClip DrinkSound;

    [SerializeField]
    Animator Slider;

    [SerializeField] GameObject PanneauUi;
    [SerializeField] GameObject looseText;
    [SerializeField] GameObject victoryText;
    
    int currentTickWinning = 0;

[SerializeField]    ULB3_rightBar rightbarent; 
    bool winBool;
    bool looseBool;
    void Start()
    {
        GameManager.Register();
        GameController.Init(this);

        AudioManager.Register();
        AudioManager.PlaySound(TavernSound, 0,6);

        PanneauUi.SetActive(false);
        looseText.SetActive(false);
        victoryText.SetActive(false);

        if (GameController.difficulty == 1 || GameController.difficulty == 2)
        {
            pointToWin = 3;
            slider.maxValue = 3;
        }

        if (GameController.difficulty == 3)
        {
            pointToWin = 4;
            slider.maxValue = 4;
        }
    }

    public void OnTick()
    {      
        animBar();
        if (GameController.currentTick == 5)
        {
            if (winBool == false)
            {
                result = false;
                looseBool = true;
                GameController.StopTimer();
                PanneauUi.SetActive(true);
                looseText.SetActive(true);
                currentTickWinning = GameController.currentTick;
            }
        }

        if (winBool == true && looseBool == false && currentTickWinning==0)
        {
            result = true;
            GameController.StopTimer();
            currentTickWinning = GameController.currentTick;
        }

        if (GameController.currentTick == currentTickWinning + 3 && currentTickWinning != 0)
        {
            GameController.FinishGame(result);
        }



        if (AxisUpConfirmed == true && AxisDownConfirmed == true)
        {
            BrasYov.SetTrigger("PlayAnim");
            TimerAnimYov();
            Slider.SetTrigger("PlayAnim");
            TimerAnimSlider();
            AudioManager.PlaySound(DrinkSound);
            point++;
            AxisUpConfirmed = false;
            AxisDownConfirmed = false;
        }


    }


    private void Update()
    {
        if (looseBool == false && winBool == false)
        {
            if (AxisDownConfirmed == false || AxisUpConfirmed == false && rightbarent.rightBarTouched == true)
            {
                if (InputManager.GetAxis(ControllerAxis.LEFT_STICK_VERTICAL) > 0.5f)
                {
                    AxisUpConfirmed = true;


                }
                if (InputManager.GetAxis(ControllerAxis.LEFT_STICK_VERTICAL) > -0.5f)
                {
                    AxisDownConfirmed = true;

                }
            }
        }
      

        if (point >= pointToWin && looseBool == false && !winBool)
        {
            winBool = true;
            PanneauUi.SetActive(true);
            victoryText.SetActive(true);
            Debug.Log("Gagné");
        }

        slider.value = point;
    }
    public void animBar()
     { 
        leftBar.Play("leftBar");
        rightBar.Play("rightBar");
     }

  IEnumerator TimerAnimYov()
    {
        yield return new WaitForSeconds(0.2f);
        BrasYov.SetBool("PlayAnimation", false);

    }
    IEnumerator TimerAnimSlider()
    {
        yield return new WaitForSeconds(0.2f);
        Slider.SetBool("PlayAnimation", false);

    }
}