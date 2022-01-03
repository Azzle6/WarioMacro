using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ULA_Succed : MonoBehaviour
{
    [SerializeField] private Animation anim;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            anim.Play();
            ULA_SoundManager.instance.PlaySound(1);
        }
    }
}
