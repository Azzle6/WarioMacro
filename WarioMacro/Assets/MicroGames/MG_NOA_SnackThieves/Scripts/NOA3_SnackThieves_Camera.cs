using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class NOA3_SnackThieves_Camera : MonoBehaviour, ITickable
{
    public bool unZoom;
    private int currentDifficulty;
    [SerializeField] private Camera cam;

    
    private void Start()
    {
        GameManager.Register();
        GameController.Init(this);
        currentDifficulty = GameController.difficulty;
        if (currentDifficulty == 3)
        {
            transform.localScale = new Vector3(transform.localScale.x/2,transform.localScale.y/2,transform.localScale.z/2);
        }
        
    }

    public void OnTick()
    {
        if (GameController.currentTick == 0)
        {
            cam.fieldOfView = 60;
        }
    }

    private void FixedUpdate()
    {
        if ((cam.fieldOfView > 25) && (unZoom == false))
        {
            cam.fieldOfView -= 1;
        }
        
        if ((cam.fieldOfView < 60) && (unZoom == true))
        {
            cam.fieldOfView += 1;
        }
    }
}
