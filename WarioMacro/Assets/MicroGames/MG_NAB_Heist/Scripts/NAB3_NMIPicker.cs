using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NAB3_NMIPicker : MonoBehaviour
{
    public Sprite[] enemyList;
    SpriteRenderer sr;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>(); 
        sr.sprite = enemyList[Random.Range(0, enemyList.Length)];
    }
    void Update()
    {
        
    }
}
