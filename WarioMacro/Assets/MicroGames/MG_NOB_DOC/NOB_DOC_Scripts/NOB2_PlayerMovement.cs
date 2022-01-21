using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NOB2_PlayerMovement : MonoBehaviour
{
    public static NOB2_PlayerMovement instance;
    [SerializeField] private GameObject finishLine;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] sprites = new Sprite[2];
    private bool spriteState;
    [SerializeField] private float speed;
    [SerializeField] private float horizontalAxisValue;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == finishLine && NOB2_GameManager.instance.resultPending)
        {
            NOB2_GameManager.instance.SetResult(true);
        }
    }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        StartCoroutine(RunAnimation());
    }
    private void Update()
    {
        horizontalAxisValue = InputManager.GetAxis(ControllerAxis.LEFT_STICK_HORIZONTAL);
    }

    void FixedUpdate()
    {
        if (horizontalAxisValue != 0 && NOB2_GameManager.instance.resultPending)
        {
            transform.Translate(new Vector2(horizontalAxisValue * speed, 0));
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, -8, 8), transform.position.y,
                transform.position.z);
        }
    }

    IEnumerator RunAnimation()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.25f);
            if (horizontalAxisValue != 0 && NOB2_GameManager.instance.resultPending)
            {
                if (spriteState)
                {
                    spriteRenderer.sprite = sprites[0];
                }
                else
                {
                    spriteRenderer.sprite = sprites[1];
                }
                spriteState = !spriteState;
            }
        }
       
    }
}
