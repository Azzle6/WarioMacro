using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Node currentNode;

   /* void Update()
    {
        if (currentNode != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, currentNode.transform.position, Time.deltaTime * 10f);
        }
    }*/

    // [SerializeField] private Rigidbody2D rb;


    /*void Start()
    {
    }

    public void Move(Vector3 des)
    {
        rb.velocity = Vector2.zero;
        rb.velocity = (des - rb.transform.position).magnitude * (des - rb.transform.position).normalized;
    }

    public void StopMoving()
    {
        rb.velocity = Vector2.zero;
    }*/

}
