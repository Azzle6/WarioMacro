using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NAC2_MoveBall : MonoBehaviour
{
    private int ballNbTick;
    [SerializeField] private float speed;
    private float yOffset;

    [SerializeField] private bool spawnRight;
    private float xStartPos;
    public bool canMove = false;

    

    private void Start()
    {
        if (spawnRight)
        {
            xStartPos = 40;
            speed = -speed;
        }
        else 
        {
            xStartPos = -40;
        } 
    }

    private Vector3 newPos;
    private void Update()
    {

        if (canMove)
        {
            xStartPos += Time.deltaTime * speed;

            newPos = new Vector3(xStartPos, (float)-(xStartPos * xStartPos * 0.02) + yOffset, 0);
            transform.position = newPos;
        }

    }

    public void SetYOffset(float yOffset) 
    {
        this.yOffset = yOffset;
    }

    public void SetCanMove(bool canMove) 
    {
        this.canMove = canMove;
    }
}
