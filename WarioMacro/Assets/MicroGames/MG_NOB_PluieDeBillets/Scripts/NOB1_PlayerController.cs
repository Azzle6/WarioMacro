using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NOB1_PlayerController : MonoBehaviour
{
    public float speed = 5;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Déplacement
        if (InputManager.GetAxis(ControllerAxis.LEFT_STICK_HORIZONTAL) > 0.5f)
        {
            transform.Translate(Vector3.right * Time.deltaTime * speed);
        } else if (InputManager.GetAxis(ControllerAxis.LEFT_STICK_HORIZONTAL) < -0.5f)
        {
            transform.Translate(Vector3.left * Time.deltaTime * speed);
        }
    }
}
