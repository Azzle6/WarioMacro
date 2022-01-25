using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ULC1_Phatass_PinceController : MonoBehaviour, ITickable
{   
    [SerializeField] private float speed = 5f;
    public float endingTimeModifier;
    private Vector2 joystickInput;
    [SerializeField] private bool canTakeKey = false, hasPinched = false, bestEnding, canInput, canStopTimer = false, canStopGame = false;
    public bool phatassSuccess;
    [SerializeField] Sprite neutralPince, readyPince;
    [SerializeField] private SpriteRenderer thisSpriteRenderer;
    [SerializeField] private PlayableDirector soundDirector;
    [SerializeField] private Animator phatassAnimator;
    // Fail Audio Clips
    [SerializeField] AudioClip screamingSquirel, wilhelmScream, yameteKudasai, uCantTouchThis, getOverHere, weGotEm;
    // Success Audio Clips
    [SerializeField] AudioClip gotchaBitch;
    [SerializeField] private PlayableDirector gtaMissionPassed, jojoReference;
    AudioSource ahhhhSource;


    private void Start()
    {
        canInput = true;
        bestEnding = true;
        ahhhhSource = GetComponent<AudioSource>();
        PrepareEndingTime();
        GameManager.Register(); //Mise en place du Input Manager, du Sound Manager et du Game Controller
        GameController.Init(this); //Permet a ce script d'utiliser le OnTick
    }

    void Update()
    {
        if (canInput)
        {
            joystickInput = new Vector2(InputManager.GetAxis(ControllerAxis.LEFT_STICK_HORIZONTAL),-InputManager.GetAxis(ControllerAxis.LEFT_STICK_VERTICAL));
        
            transform.position += new Vector3(joystickInput.x, -joystickInput.y, 0) * speed * GameController.gameBPM/60;
        
            if (InputManager.GetKeyDown(ControllerKey.A) && canTakeKey && !hasPinched) 
            {
                phatassSuccess = true;
                canStopTimer = true;
                /*if (bestEnding)
                {
                phatassAnimator.SetTrigger("RespecMaDude");
                gtaMissionPassed.enabled = true;
                thisSpriteRenderer.enabled = false;
                hasPinched = true;
                soundDirector.enabled = false;
                }*/
                int randomValue = UnityEngine.Random.Range(1, 4);
                if (randomValue == 1)
                {
                    phatassAnimator.SetTrigger("SnipTheKey");
                    ahhhhSource.clip = gotchaBitch;
                    ahhhhSource.Play();
                    thisSpriteRenderer.enabled = false;
                    hasPinched = true;
                    soundDirector.enabled = false;
                    StartCoroutine(HolupALilBit(1f * endingTimeModifier));
                }
                else if (randomValue == 2)
                {
                    phatassAnimator.SetTrigger("SnipTheKey");
                    ahhhhSource.clip = getOverHere;
                    ahhhhSource.Play();
                    thisSpriteRenderer.enabled = false;
                    hasPinched = true;
                    soundDirector.enabled = false;
                    StartCoroutine(HolupALilBit(1.75f * endingTimeModifier));
                }
                else if (randomValue == 3)
                {
                    phatassAnimator.SetTrigger("SnipTheKey");
                    ahhhhSource.clip = weGotEm;
                    ahhhhSource.Play();
                    thisSpriteRenderer.enabled = false;
                    hasPinched = true;
                    soundDirector.enabled = false;
                    StartCoroutine(HolupALilBit(2.2f * endingTimeModifier));
                }
                 
            }
        else if (InputManager.GetKeyDown(ControllerKey.A) && !canTakeKey && !hasPinched)
        {
            phatassSuccess = false;
            canStopTimer = true;
            /*if (bestEnding)
            {
                phatassAnimator.SetTrigger("ToBeContinued");
                jojoReference.enabled = true;
                //ahhhhSource.clip = jojoReference;
                //ahhhhSource.Play();
                
                thisSpriteRenderer.enabled = false;
                hasPinched = true;
                soundDirector.enabled = false;
            }*/
            
            int randomValue = UnityEngine.Random.Range(1, 5);
            if(randomValue == 1)
            {
                phatassAnimator.SetTrigger("OhYeahPinchIt");
                ahhhhSource.clip = screamingSquirel;
                ahhhhSource.Play();
                thisSpriteRenderer.enabled = false;
                hasPinched = true;
                soundDirector.enabled = false;
                StartCoroutine(HolupALilBit(1.5f * endingTimeModifier));
            }            
            else if(randomValue == 2)
            {
                phatassAnimator.SetTrigger("OhYeahPinchIt");
                ahhhhSource.clip = wilhelmScream;
                ahhhhSource.Play();
                thisSpriteRenderer.enabled = false;
                hasPinched = true;
                soundDirector.enabled = false;
                StartCoroutine(HolupALilBit(1f * endingTimeModifier));
            }
            else if(randomValue == 3)
            {
                phatassAnimator.SetTrigger("OhYeahPinchIt");
                ahhhhSource.clip = uCantTouchThis;
                ahhhhSource.Play();
                thisSpriteRenderer.enabled = false;
                hasPinched = true;
                soundDirector.enabled = false;
                StartCoroutine(HolupALilBit(.75f * endingTimeModifier));
            }
            else if(randomValue == 4)
            {
                phatassAnimator.SetTrigger("OhYeahPinchIt");
                ahhhhSource.clip = yameteKudasai;
                ahhhhSource.Play();
                thisSpriteRenderer.enabled = false;
                hasPinched = true;
                soundDirector.enabled = false;
                StartCoroutine(HolupALilBit(1.5f * endingTimeModifier));
            }
        }
        } 
    }

    void PrepareEndingTime()
    {
        if(GameController.gameBPM == 50)
        {
            endingTimeModifier = .9f;
        }
        else if(GameController.gameBPM == 60)
        {
            endingTimeModifier = 1.1f;
        }
        else if (GameController.gameBPM == 70)
        {
            endingTimeModifier = 1.3f;
        }
        else if (GameController.gameBPM == 80)
        {
            endingTimeModifier = 1.5f;
        }
    }

    IEnumerator HolupALilBit(float yieldTime)
    {
        yield return new WaitForSeconds(yieldTime);
        canStopGame = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("clef"))
        {
            canTakeKey = true;
            thisSpriteRenderer.sprite = readyPince;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("clef"))
        {
            canTakeKey = false;
            thisSpriteRenderer.sprite = neutralPince;
        }
    }
    
    public void OnTick()
    {
        /*if (GameController.currentTick == 3) //NO MORE LONG ENDINGS
        {
            bestEnding = false;
        }*/
        
        if (GameController.currentTick == 5) //NO MORE INPUT
        {
            GameController.StopTimer();
            soundDirector.enabled = false;
            canInput = false;
            phatassAnimator.SetTrigger("WalkAway");
            Debug.Log("No more Input");
        }
        
        if (canStopTimer)
        {
            GameController.StopTimer();
            canStopTimer = false;
        }

        if (canStopGame)
        {
            canStopGame = false;
            GameController.FinishGame(phatassSuccess);
        }

        if (GameController.currentTick == 8) //FINISH GAME
        {
            GameController.FinishGame(phatassSuccess);
        }
    }
}
