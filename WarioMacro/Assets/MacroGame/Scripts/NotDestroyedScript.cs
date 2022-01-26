using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotDestroyedScript : MonoBehaviour
{
    public static NotDestroyedScript instance;
    public static bool isAReload = false;
    
    private void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("PersistentObject");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }
        
        DontDestroyOnLoad(this.gameObject);
        instance = this;
    }

    public void EndRun(bool win)
    {
        StartCoroutine(GameController.instance.ToggleEndGame(win));
    }
}
