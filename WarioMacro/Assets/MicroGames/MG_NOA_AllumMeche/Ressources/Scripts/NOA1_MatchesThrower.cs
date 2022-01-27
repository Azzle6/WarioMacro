using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class NOA1_MatchesThrower : MonoBehaviour
{
    [SerializeField] private float speed  = 2;
    [SerializeField] private Animator animator;
    private Rigidbody lastMatches;
    [SerializeField] private GameObject matches;
    [SerializeField] private Transform spawnPos;
    [SerializeField] private Vector2 forceDirection;
    [SerializeField] private Vector2 forceRange;
    private bool started = false;

    private void OnEnable()
    {
        animator.SetFloat("Speed", speed);
        StartCoroutine(WaitForStart());
    }

    void Update()
    {
        if ((InputManager.GetKeyDown(ControllerKey.A)) || (InputManager.GetKeyDown(ControllerKey.B)) || (InputManager.GetKeyDown(ControllerKey.X)) || (InputManager.GetKeyDown(ControllerKey.Y)))
        {
            if (started)
            {
                lastMatches = Instantiate(matches).GetComponent<Rigidbody>();
                lastMatches.transform.position = spawnPos.position;
                lastMatches.AddForce(forceDirection*Random.Range(forceRange.x,forceRange.y),ForceMode.Impulse);
            }
            
        }
    }
    
    IEnumerator WaitForStart()
    {
        yield return new WaitForSeconds(0.2f);
        started = true;
    }
}
