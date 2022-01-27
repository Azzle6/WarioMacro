using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ULB2_Slider : MonoBehaviour, ITickable
{
    public Animator animator, animatorPlayer, animatorJoueur, animatorTime;
    public Animation animSlider, jumpFail, jumpSuces;
    public Slider slider;
    float speed = 1;
    float sliderValue = 0f;
    public RectTransform GoodZone, handle;
    bool colider = false;
    Vector2 zonePosition;
    public GameControllerSO difficulty;
    public int[] sizes;
    private bool result = false;
    private bool hasJumped;
    public AudioSource[] audio;
    public static ULB2_Slider instance;
    private int actionTick = -1;


    //public nomduscript (nom de la variable) puis drag n drop nom variable.playidleanim

    public bool launch = false;

    // Start is called before the first frame update
    void Start()
    {
        if (ULB2_Slider.instance != null)
        {
            Destroy(this);
        }

        instance = this;

        GameManager.Register();
        GameController.Init(this);

        float offSet = 0;

        if (difficulty.currentDifficulty == 1)
        {
            GoodZone.sizeDelta = new Vector2(GoodZone.rect.width, sizes[0]);
        }
        if (difficulty.currentDifficulty == 2)
        {
            GoodZone.sizeDelta = new Vector2(GoodZone.rect.width, sizes[1]);
        }
        if (difficulty.currentDifficulty == 3)
        {
            GoodZone.sizeDelta = new Vector2(GoodZone.rect.width, sizes[2]);
        }

        slider.value = 0f;

        int goodZonePosition = Random.Range(-79, 116);
        if (goodZonePosition < 0f)
        {
            offSet = GoodZone.rect.height/2;
;        }
        else
        {
            offSet = GoodZone.rect.height / 2 * -1;
        }

        GoodZone.localPosition = new Vector3(GoodZone.localPosition.x, goodZonePosition + offSet, GoodZone.localPosition.z);

        //GoodZone.pivot = new Vector2(0.5f, 0);
        zonePosition.x = GoodZone.localPosition.y - Mathf.Abs(offSet);
        //GoodZone.pivot = new Vector2(0.5f, 0.5f);
        zonePosition.y = GoodZone.localPosition.y + Mathf.Abs(offSet);
        Debug.Log(zonePosition);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.currentTick >= 5)
        {
            result = false;
            return;
        }

        if (InputManager.GetAxis(ControllerAxis.LEFT_TRIGGER)!=0 || InputManager.GetAxis(ControllerAxis.RIGHT_TRIGGER) != 0 && !launch)
        {
            launch = true;
            animator.SetBool("launch", true);
        }
        if (InputManager.GetAxis(ControllerAxis.LEFT_TRIGGER) ==0 && InputManager.GetAxis(ControllerAxis.RIGHT_TRIGGER) ==0 && launch)
        {
            animator.speed = 0f;
            sliderValue = slider.value;
            //GRL_Audio.instance.audio[0].Stop();
            TestValue();
            Debug.Log("handle : " + handle.localPosition);
        }
    }

    void TestValue()
    {
        if (hasJumped)
        {
            return;
        }

        if (zonePosition.x <= handle.localPosition.y && handle.localPosition.y <= zonePosition.y)
        {
            //animatorPlayer.SetTrigger("bon");
            animatorPlayer.Play("Jump");
            //animatorJoueur.SetTrigger("jump");
            animatorJoueur.Play("jump_anim");
            result = true;

        }
        else
        {
            //animatorPlayer.SetTrigger("mauvais");
            animatorPlayer.Play("Fail");
            //animatorJoueur.SetTrigger("death");
            animatorJoueur.Play("death-anim");
            result = false;
        }

        actionTick = Mathf.Clamp(GameController.currentTick + 1, 0, 5);
        hasJumped = true;
        
    }

    public void OnTick()
    {
        if (actionTick != -1)
        {
            Debug.Log(actionTick);
            if (GameController.currentTick == actionTick)
            {
                GameController.StopTimer();
            }

            if(GameController.currentTick == actionTick + 3)
            {
                GameController.FinishGame(result);
                return;
            }
        }
        else
        {
            if (GameController.currentTick == 5)
            {
                GameController.StopTimer();
                animatorTime.SetTrigger("endGame");
                Play(4);
            }

            if (GameController.currentTick == 8)
            {
                GameController.FinishGame(result);
            }
        }

    }

    public void Play(int index)
    {
        audio[index].Play();
    }

    public void Stop(int index)
    {
        audio[index].Stop();
    }
}
