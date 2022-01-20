
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NOA1_DropSpawner : MonoBehaviour, ITickable
{

    [SerializeField] private bool[] tick;
    [SerializeField] private GameObject drop;
    [SerializeField] private AudioClip dropSFX;
    private GameObject lastDrop;

    private void Start()
    {
        GameManager.Register();
        GameController.Init(this);
    }

    public void OnTick()
    {
        if (GameController.currentTick < 6)
        {
            if (tick[GameController.currentTick])
            {
                SpawnStart();
            }
        }
        
    }
    private void SpawnStart()
    {
        AudioManager.PlaySound(dropSFX);
        lastDrop = Instantiate(drop);
        lastDrop.transform.position = transform.position;
    }
}
