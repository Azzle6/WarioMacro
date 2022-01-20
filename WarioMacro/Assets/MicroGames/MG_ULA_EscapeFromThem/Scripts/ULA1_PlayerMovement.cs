using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ULA1_PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed;
    private float direction;
    private int statement = 0;
    void Update()
    {
        if (!ULA1_GameManager.instance.cinematic)
        {
            NormalizeAxis();
            ChangeMovement();
            UpdateStatut();
        }else
        {
            rb.velocity = Vector3.zero;
        }

    }

    void UpdateStatut()
    {
        if (statement == 0)
        {
            rb.velocity = Vector3.forward * speed;
        }else if (statement == 1)
        {
            rb.velocity = new Vector3(direction,0, 0) * speed;
        }
    }

    void ChangeMovement()
    {
        if (direction!=0)
        {
            statement = 1;
        }
    }

    void NormalizeAxis()
    {
        if (statement == 0)
        {
            if (InputManager.GetAxis(ControllerAxis.LEFT_STICK_VERTICAL) > 0.1)
            {
                direction = -1;
                rb.transform.Rotate(0,-90,0);
            }
            else if (InputManager.GetAxis(ControllerAxis.LEFT_STICK_VERTICAL) < -0.1)
            {
                direction = 1;
                rb.transform.Rotate(0,90,0);
            }
        }
    }
}
