using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NOA1_DropSpawnerManager : MonoBehaviour
{
    [SerializeField] private GameObject[] dropSpawner;
    private int currentDifficulty;

    private void Start()
    {
        GameManager.Register();
        currentDifficulty = GameController.difficulty;
        if (currentDifficulty > 0)
        {
            dropSpawner[1].SetActive(true);
        }
        if (currentDifficulty > 1)
        {
            dropSpawner[2].SetActive(true);
        }
        if (currentDifficulty >2)
        {
            dropSpawner[0].SetActive(true);
            dropSpawner[3].SetActive(true);
        }
       
    }
}
