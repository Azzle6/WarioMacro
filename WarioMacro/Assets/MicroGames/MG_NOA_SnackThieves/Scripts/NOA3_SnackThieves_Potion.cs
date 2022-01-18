using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class NOA3_SnackThieves_Potion : MonoBehaviour
{
    private int currentDifficulty;
    [SerializeField] private float speed;
    private bool way;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Animator snackAnimator;
    [SerializeField] private Animator potionAnimator;
    
    private void Start()
    {
        GameManager.Register();
        currentDifficulty = GameController.difficulty;
        if (currentDifficulty == 3)
        {
            potionAnimator.SetBool("Small",true);
        }
        if ((currentDifficulty > 1))
        {
           snackAnimator.SetFloat("Speed", speed);
        }
    }
    

    private void OnCollisionEnter(Collision other)
    {
        way = !way;
    }
}
