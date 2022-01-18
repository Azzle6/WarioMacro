using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NOA3_SnackThieves_HandMouvement : MonoBehaviour, ITickable
{
    [SerializeField] private float speed  =2;
    [SerializeField] private float speedUp  = 2;
    [SerializeField] private bool go;
    private bool results;
    private bool isFinished;
    private int tickEnd = 10;
    [SerializeField] private AudioClip[] audioClips;
    [SerializeField] private bool way;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private NOA3_SnackThieves_Camera cam;
    [SerializeField] private GameObject winScreen;
    [SerializeField] private Animator animatorPotion;
    [SerializeField] private Animator animatorSorcier;
    [SerializeField] private ParticleSystem winParticle;
    

    private void Start()
    {
        GameManager.Register();
        GameController.Init(this);
    }

    public void OnTick()
    {
        Debug.Log(tickEnd);
        if (GameController.currentTick == 5 && !isFinished)
        {
            if (results == false)
            {
                tickEnd = GameController.currentTick;
                GameController.StopTimer();
                MiniGameLose();
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

    void Update()
    {
        if (InputManager.GetKeyDown(ControllerKey.A))
        {
            go = true;
        }
        
        if (go == false)
        {
            if (way == true)
            {
                rb.velocity = new Vector3(0,0,speed);
            }

            if (way == false)
            {
                rb.velocity = new Vector3(0,0,-speed);
            }
        }
        if (go == true)
        {
            rb.velocity = new Vector3(0, speedUp, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "PotionHappy")
        {
            isFinished = true;
            results = true;
            winScreen.SetActive(true);
            animatorSorcier.gameObject.SetActive(false);
            animatorPotion.SetBool("end", true);
            winParticle.Play();
            cam.unZoom = true;
            gameObject.SetActive(false);
            AudioManager.PlaySound(audioClips[2]);
            AudioManager.PlaySound(audioClips[1],1,0.6f);
        }
        

        if (other.gameObject.name == "Lose")
        {
           MiniGameLose();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        way = !way;
        
    }

    private void MiniGameLose()
    {
        isFinished = true;
        AudioManager.PlaySound(audioClips[0]);
        animatorPotion.SetBool("lose", true);
        animatorSorcier.SetBool("go", true);
        cam.unZoom = true;
        gameObject.SetActive(false);
    }
}
