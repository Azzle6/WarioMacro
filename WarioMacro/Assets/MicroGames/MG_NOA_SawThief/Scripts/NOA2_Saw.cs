using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class NOA2_Saw : MonoBehaviour, ITickable
{
    [SerializeField] GameObject headFollow;
    [SerializeField] private bool moving;
    [SerializeField] private Transform leftPt;
    [SerializeField] private Transform rightPt;

    [SerializeField] private Transform pointSaw;
    
    [SerializeField] private Transform downPt;
    [SerializeField] private Transform downLeft;
    [SerializeField] private Transform downRight;

    [SerializeField] private Transform cutPointLeft;
    [SerializeField] private Transform cutPointRight;
    [SerializeField] private Transform[] cutHere;
    //[SerializeField] Transform cutGroup;

    [SerializeField] private Animator head;
    [SerializeField] private Animator body;
    [SerializeField] private Animator grille;
    [SerializeField] private Animator winHead;
    [SerializeField] private Animator coffre;
    [SerializeField] Animator scaler;
    
    
    [SerializeField] private Image stayGuy;
    [SerializeField] private Image cutGuy;

    [SerializeField] private GameObject coinParticles;

    [SerializeField] AudioClip sawingSound;
    [SerializeField] AudioClip coinsDrop;
    
    public bool results;
    private bool inverted;
    bool fail;
    int stopTick;
    
    
    [SerializeField] private float tValue;
    
   private Vector3 a;
   private Vector3 b;
   private Vector3 e;
   private Vector3 f;
   private Vector3 g;
   private Vector3 h;
   private Vector3 i;
   private Vector3 j;
    
    private  float t1;
    private float cutMin;
    private float cutMax;

    bool doOnce = true;
    bool doOnceTick = true;
    
    private void Start()
    {
        GameManager.Register();
        GameController.Init(this);
        //Instantiation pour les lerps présent à foison dans ce script
        a = new Vector3(leftPt.position.x, leftPt.position.y, leftPt.position.z);
        b = new Vector3(rightPt.position.x, rightPt.position.y, rightPt.position.z);
        e = new Vector3(downLeft.position.x, downLeft.position.y, downLeft.position.z);
        f = new Vector3(downRight.position.x, downRight.position.y, downRight.position.z);
        i = new Vector3(cutPointLeft.position.x, cutPointLeft.position.y, cutPointLeft.position.z);
        j = new Vector3(cutPointRight.position.x, cutPointRight.position.y, cutPointRight.position.z);

        
        float t = Random.Range(0.1f, 0.9f);
        var position = Vector3.Lerp(i, j, t);
        
        //Gère la mise en place des indications où couper
        cutHere[0].position = position;

        if (t >= 0.8 && GameController.difficulty == 1)
        {
                var position2 = Vector3.Lerp(i, j, t - 0.2f);
                cutHere[1].position = position2;
                cutMax = t;
                cutMin = t - 0.2f;
                inverted = true;
              //print(cutMin);
              //print(cutMax);
        }
        
        else if (t < 0.8 && GameController.difficulty == 1)
        {
                var position2 = Vector3.Lerp(i, j, t + 0.2f);
                cutHere[1].position = position2;
                cutMax = t + 0.2f;
                cutMin = t;
                inverted = false;
              //print(cutMin);
              //print(cutMax);
        }
        
        if (t >= 0.8 && (GameController.difficulty == 2 || GameController.difficulty ==3))
        {
                var position2 = Vector3.Lerp(i, j, t - 0.15f);
                cutHere[1].position = position2;
                cutMax = t;
                cutMin = t - 0.15f;
                inverted = true;
               //print(cutMin);
               //print(cutMax);
        }
        
        else if (t < 0.8 && (GameController.difficulty == 2 || GameController.difficulty ==3))
        {
                var position2 = Vector3.Lerp(i, j, t + 0.15f);
                cutHere[1].position = position2;
                cutMax = t + 0.15f;
                cutMin = t;
                inverted = false;
               /* print(cutMin);
                print(cutMax);*/
        }

    }

    private Vector3 velocity;
    private bool firtSawMove;
    void Update()
    {
        //Verification constante de la position des 2 points permettant le mouvement haut et bas
        g = new Vector3(pointSaw.position.x, pointSaw.position.y, pointSaw.position.z);
        h = new Vector3(downPt.position.x, downPt.position.y, downPt.position.z);
        
        changeDirection(transform,a,b);
        //changeDirection(sawUpPart, c, d);
        changeDirection(downPt,e,f);
        changeDirection(pointSaw,a,b);
        SawActive();

        if (GameController.difficulty == 3 && moving)
        {
            //print("dif 3");
            CutMovement();
        }

        if (moving == false && GameController.currentTick <= 5)
        {
            if (!firtSawMove)
            {
                firtSawMove = true;
                sawT = 0;
            }
            sawMove();
        }
        
    }

    public void OnTick()
    {
        print(GameController.currentTick);
        
        if (GameController.currentTick == 5)
        {
            if (doOnce)
            {
                lose();
            }
            GameController.StopTimer();
        }
        
        if (GameController.currentTick == 8)
        {
            GameController.FinishGame(results);
        }
        
        if (results || fail)
        {
            if (doOnceTick)
            {
                stopTick = GameController.currentTick;
                doOnceTick = false;
            }

            print("stoptick :" + stopTick);
            GameController.StopTimer();

            if (GameController.currentTick == stopTick + 3)
            {
                GameController.FinishGame(results);
            }
        }
    }
    
    void changeDirection(Transform sawpart, Vector3 a, Vector3 b)
    {
       if (moving && GameController.currentTick <5)
       {
           float t = Mathf.PingPong(Time.time, 1f);
           //print(t);
           
           sawpart.position = Vector3.Lerp(a,b,t);

           tValue = t;
       }
    }

    void SawActive()
    {
        if ((InputManager.GetKeyDown(ControllerKey.A) || InputManager.GetKeyDown(ControllerKey.B) || InputManager.GetKeyDown(ControllerKey.X) || InputManager.GetKeyDown(ControllerKey.Y)) && GameController.currentTick < 5)
        {
            sawAValue(tValue);
            moving = false;

            if (doOnce)
            {
                doOnce = false;
                AudioManager.PlaySound(sawingSound, 0.5f);
            }
        }
    }

    private float sawT;
    void sawMove()
    {
        sawT += Time.deltaTime * 2f; 
        
        transform.position = Vector3.Lerp(g, h, sawT);
    }
    void sawAValue(float t)
    {

       /* print(pointSaw.position.x);
        print(cutHere[0].position.x);
        print(cutHere[1].position.x);
        print(inverted);*/
        if (t <= 0.5f)
        {
            stayGuy.fillAmount = t;
            cutGuy.fillAmount =  1 - t;
        }

        else if (t > 0.5f)
        {
            stayGuy.fillAmount = t;
            cutGuy.fillAmount =  1 - t;
        }

        if (pointSaw.position.x > cutHere[0].position.x && pointSaw.position.x < cutHere[1].position.x && !inverted)
        {
            win();
        }
        
        else if (!inverted)
        {
            lose();
        }

        if (pointSaw.position.x < cutHere[0].position.x && pointSaw.position.x > cutHere[1].position.x && inverted)
        {
            win();
        }

        else if (inverted)
        {
            lose();
        }
        
        //print("results : " + results);
        
    }

    void win()
    {
        results = true;
        head.SetTrigger("Win");
        grille.SetBool("Results", results);
        StartCoroutine(winAnimation());
    }

    void lose()
    {
        fail = true;
        headFollow.GetComponent<NOA2_PosLink>().enabled = false;
        //Debug.Log(results);
        head.SetTrigger("Lose");
        StartCoroutine(losecorout());
    }

    IEnumerator losecorout()
    {
        yield return new WaitForSeconds(0.8f);
        scaler.SetTrigger("Lose");
    }
    
    IEnumerator winAnimation()
    {
        yield return new WaitForSeconds(0.2f);
        body.SetTrigger("Win");
        yield return new WaitForSeconds(0.8f);
        winHead.SetTrigger("Win");
        yield return new WaitForSeconds(0.3f);
        scaler.SetTrigger("Win");
        yield return new WaitForSeconds(0.1f);
        coffre.SetTrigger("Win");
        yield return new WaitForSeconds(0.5f);
        coinParticles.SetActive(results);
        AudioManager.PlaySound(coinsDrop, 0.5f);
        /* return new WaitForSeconds(1.5f);
        GameController.FinishGame(results);*/
    }

    void CutMovement()
    {
        if (GameController.currentTick <= 5)
        {
            float r = Random.Range(0.2f, 0.5f);
            float t = Mathf.PingPong(Time.time, 1f);
            //print(t);

            //cutGroup.position = Vector3.SmoothDamp(cutGroup.position, Vector3.Lerp(i, j, t), ref velocity, r, 25f);
            cutHere[0].position = Vector3.SmoothDamp(cutHere[0].position, Vector3.Lerp(i, j, t), ref velocity, r, 25f);
            cutHere[1].position = Vector3.SmoothDamp(cutHere[1].position, Vector3.Lerp(i, j, t), ref velocity, r, 25f);
        }
    }
}


