using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class NOC2_Controller : MonoBehaviour
{
    public static NOC2_Controller instance;
    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private float speed = 1;

    [HideInInspector] public bool blocked;

    private Vector2 joystickValue;
    // Start is called before the first frame update
    void Start()
    {
        if (NOC2_Controller.instance != null) Destroy(gameObject);
        else instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (!blocked) Move();
        else rb.velocity = Vector2.zero;
    }

    private void Move()
    {
        joystickValue = Vector2.zero;

        if (InputManager.GetKey(ControllerKey.DPAD_LEFT)) joystickValue.x = -1;
        else if (InputManager.GetKey(ControllerKey.DPAD_RIGHT)) joystickValue.x = 1;
        if (InputManager.GetKey(ControllerKey.DPAD_DOWN)) joystickValue.y = -1;
        else if (InputManager.GetKey(ControllerKey.DPAD_UP)) joystickValue.y = 1;

        joystickValue += new Vector2(InputManager.GetAxis(ControllerAxis.LEFT_STICK_HORIZONTAL), InputManager.GetAxis(ControllerAxis.LEFT_STICK_VERTICAL));
        joystickValue.Normalize();
        if (Time.timeScale != 0) joystickValue *= 1 / Time.timeScale;
        rb.velocity = joystickValue * speed * Time.deltaTime;
        Debug.Log(joystickValue);
        
        if (rb.velocity.y > 0) NOC2_JetPack.instance.strength = 0.05f;
        else if (rb.velocity.y == 0) NOC2_JetPack.instance.strength = 0.025f; 
        else NOC2_JetPack.instance.strength = 0.01f;

        if (rb.velocity.x > 0)
        {
            foreach (SpriteRenderer s in GetComponentsInChildren<SpriteRenderer>())
            {
                s.flipX = false;
            }
        }
        else if (rb.velocity.x < 0)
        {
            foreach (SpriteRenderer s in GetComponentsInChildren<SpriteRenderer>())
            {
                s.flipX = true;
            }
        }
    }
}
