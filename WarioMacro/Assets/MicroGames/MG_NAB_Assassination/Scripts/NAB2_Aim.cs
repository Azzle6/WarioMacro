using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NAB2_Aim : MonoBehaviour
{
    public float moveSpeed;

    public Rigidbody rb;

    private Vector3 movement;
    public NAB2_GameManager gameManager;
    public NAB2_Shot shot;
    
    void Update()
    {
        if (gameManager.gameIsOver == false && shot.shotFired == false)
        {
            movement.x = InputManager.GetAxis(ControllerAxis.LEFT_STICK_HORIZONTAL);
            movement.y = InputManager.GetAxis(ControllerAxis.LEFT_STICK_VERTICAL);
        }
        if(shot.shotFired == true)
        {
            movement.x = 0;
            movement.y = 0;
        }

    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
