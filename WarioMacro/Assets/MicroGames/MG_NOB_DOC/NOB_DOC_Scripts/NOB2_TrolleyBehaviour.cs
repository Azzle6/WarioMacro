using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NOB2_TrolleyBehaviour : MonoBehaviour
{
    [SerializeField] private AudioClip rollingClip;
    [SerializeField] private GameObject finishLine;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float speed;
    public bool shouldStop = false;
    public bool stopped;

    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == finishLine && NOB2_GameManager.instance.resultPending)
        {
            NOB2_GameManager.instance.SetResult(false);
        }
    }

    private void OnEnable()
    {
        AudioManager.PlaySound(rollingClip);
    }

    void Update()
    {
        if (!stopped)
        {
            rb.velocity = Vector2.right * speed;  
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }
}
