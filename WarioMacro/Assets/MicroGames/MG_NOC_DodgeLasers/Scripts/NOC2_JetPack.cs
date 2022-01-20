using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class NOC2_JetPack : MonoBehaviour
{
    public static NOC2_JetPack instance;
    [HideInInspector] public float strength = 0.025f;
    
    private void Start()
    {
        if (NOC2_JetPack.instance != null) Destroy(this);
        else instance = this;
        StartCoroutine(JetPackShake());
    }

    private IEnumerator JetPackShake()
    {
        Vector2 startPosition =transform.position;
        while (true)
        {
            transform.localPosition = startPosition + UnityEngine.Random.insideUnitCircle * strength;
            yield return null;
        }
    }
}
