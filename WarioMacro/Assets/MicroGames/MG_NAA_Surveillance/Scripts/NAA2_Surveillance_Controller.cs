using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NAA2_Surveillance_Controller : MonoBehaviour, ITickable
{

    [SerializeField]
    float baseSpeed = 5;
    Vector2 direction;
    [SerializeField]
    NAA2_Surveillance_MicroGameController mgc;
    [SerializeField] 
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        //GameManager.Register();
        //GameController.Init(this);
    }

    public void OnTick()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (mgc.gameStarted)
        {
            direction = new Vector2(InputManager.GetAxis(ControllerAxis.LEFT_STICK_HORIZONTAL), InputManager.GetAxis(ControllerAxis.LEFT_STICK_VERTICAL));

            rb.velocity = direction * baseSpeed;
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
        
    }
}
