using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NOB_PAP_InputManager : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private NOB_PAP_CrowbarUser crowbarUser;
    [SerializeField] private float horizontalAxisValue;
    [SerializeField] private float verticalAxisValue;
    [SerializeField] private float speed;
    private Vector3 movement;

    void Update()
    {
        horizontalAxisValue = InputManager.GetAxis(ControllerAxis.LEFT_STICK_HORIZONTAL);
        verticalAxisValue = InputManager.GetAxis(ControllerAxis.LEFT_STICK_VERTICAL);

        movement = new Vector3(horizontalAxisValue, verticalAxisValue, 0) * speed;
        rb.velocity = movement;

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -8.5f, 8.5f), Mathf.Clamp(transform.position.y, -4, 6),-1);

        if (InputManager.GetKeyDown(ControllerKey.A) || InputManager.GetKeyDown(ControllerKey.B) || InputManager.GetKeyDown(ControllerKey.X) || InputManager.GetKeyDown(ControllerKey.Y))
        {
            crowbarUser.Use();
        }
    }
}
