using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NAA1_Safe_QuadrantManager : MonoBehaviour
{
    private NAA1_Safe_SafeManager safeManager;

    private void Start()
    {
        safeManager = GetComponentInParent<NAA1_Safe_SafeManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (safeManager)
        {
            safeManager.chosenNumber = other.name.Length;
        }
    }
}
