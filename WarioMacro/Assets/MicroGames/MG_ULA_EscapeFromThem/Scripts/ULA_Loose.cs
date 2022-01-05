using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ULA_Loose : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Player"))
        {
            ULA_GameManager.instance.Loose();
        }
    }
}
