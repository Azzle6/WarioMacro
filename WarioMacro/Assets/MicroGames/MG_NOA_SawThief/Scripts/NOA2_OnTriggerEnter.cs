using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NOA2_OnTriggerEnter : MonoBehaviour
{
    public NOA2_Saw saw;

    private void OnTriggerEnter2D(Collider2D other)
    {
            saw.results = true;
            print("COLLISION!!!!!!!!!!!!!!");
    }
}
