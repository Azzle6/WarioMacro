using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ULA1_Loose : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Player"))
        {
            ULA1_GameManager.instance.Loose();
        }
    }
}
