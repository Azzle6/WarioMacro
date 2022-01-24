using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NAB1_VictoryManager : MonoBehaviour
{
    public static NAB1_VictoryManager instance;
    public bool victory;
    
    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
 
        instance = this;
    }
}
