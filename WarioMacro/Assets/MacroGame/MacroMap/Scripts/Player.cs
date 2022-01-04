using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Animator animator;
    public float moveSpeed = 1f;

    public void StartMove()
    {
        animator.SetTrigger("Move");
    }

    public void StopMove()
    {
        animator.SetTrigger("Idle");
    }
}
