using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ULA_Loose : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            other.transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            GameController.FinishGame(true);
        }
        
    }
}
