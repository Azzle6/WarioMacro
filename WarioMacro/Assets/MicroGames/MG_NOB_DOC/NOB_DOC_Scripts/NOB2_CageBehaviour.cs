using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NOB2_CageBehaviour : MonoBehaviour
{
    public static NOB2_CageBehaviour instance;
    [SerializeField] private Rigidbody2D rb;

    private void Awake()
    {
        instance = this;
    }

    public void Fall(Vector2 pos)
    {
        transform.position = new Vector3(pos.x, transform.position.y, transform.position.z);
        rb.gravityScale = 2.5f;
    }
}
