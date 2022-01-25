using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NAC3_PlayerInputER : MonoBehaviour
{
    [SerializeField] private NAC3_EnRappelController erController;

    private float rightTriggerAxis;
    private float xAxis;

    [SerializeField] private Transform player;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float rightSpeed;
    [SerializeField] private float fallSpeed;


    private void Update()
    {
        if (!erController.getHasEnded())
        {
            rightTriggerAxis = InputManager.GetAxisRaw(ControllerAxis.RIGHT_TRIGGER);
            xAxis = InputManager.GetAxisRaw(ControllerAxis.LEFT_STICK_HORIZONTAL);

            MovePlayer();
        }
        else 
        {
            rb.velocity = Vector3.zero;
        }
    }

    private void MovePlayer() 
    {
        rb.velocity = new Vector3(xAxis*rightSpeed, -(rightTriggerAxis) * fallSpeed, 0);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Finish")) 
        {
            erController.CheckEndGame(true);
        }
    }
}
