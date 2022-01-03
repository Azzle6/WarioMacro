using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ULA_EnnemiMovement : MonoBehaviour
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
        if (!ULA_GameManager.instance.init)
        {
            rb.velocity = Vector3.forward * speed;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            ULA_GameManager.instance.SetResult(false);
        }
    }
}
