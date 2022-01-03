using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NOB_DOC_TrolleyBehaviour : MonoBehaviour
{
    [SerializeField] private AudioClip rollingClip;
    [SerializeField] private GameObject finishLine;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float speed;
    public bool shouldStop = false;
    private bool stopped;

    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == finishLine && NOB_DOC_GameManager.instance.resultPending)
        {
            NOB_DOC_GameManager.instance.resultPending = false;
            GameController.FinishGame(false);
            Debug.Log("Lose !");
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

    IEnumerator Stop()
    {
        stopped = true;
        yield return new WaitForSeconds(1);
        stopped = false;
    }
}
