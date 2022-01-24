using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ULA1_EnnemiMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!ULA1_GameManager.instance.cinematic)
        {
            rb.velocity = Vector3.forward * speed;
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Player"))
        {
            ULA1_GameManager.instance.SetResult(false);
        }
    }
}
