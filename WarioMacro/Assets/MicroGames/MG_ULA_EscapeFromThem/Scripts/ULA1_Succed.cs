using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ULA1_Succed : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            rb.useGravity = true;
            ULA1_GameManager.instance.Win();
        }
    }
}
