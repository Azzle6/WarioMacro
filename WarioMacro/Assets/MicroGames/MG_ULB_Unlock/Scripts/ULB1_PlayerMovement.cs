using UnityEngine;

public class ULB1_PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    public float speed;

    private void Update()
    {
        Movement();
    }

    public void Movement()
    {
        if (InputManager.GetAxis(ControllerAxis.LEFT_STICK_HORIZONTAL)!=0 || InputManager.GetAxis(ControllerAxis.LEFT_STICK_VERTICAL)!=0)
        {
            rb.velocity = new Vector3(InputManager.GetAxis(ControllerAxis.LEFT_STICK_HORIZONTAL), InputManager.GetAxis(ControllerAxis.LEFT_STICK_VERTICAL), 0) * speed;
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }
}
