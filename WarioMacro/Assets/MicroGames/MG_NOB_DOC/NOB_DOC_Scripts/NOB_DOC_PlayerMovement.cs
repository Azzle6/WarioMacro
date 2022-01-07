using System.Collections;
using UnityEngine;

public class NOB_DOC_PlayerMovement : MonoBehaviour
{
    [SerializeField] private GameObject finishLine;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] sprites = new Sprite[2];
    private bool spriteState;
    [SerializeField] private float speed;
    [SerializeField] private float horizontalAxisValue;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == finishLine && NOB_DOC_GameManager.instance.resultPending)
        {
            NOB_DOC_GameManager.instance.SetResult(true);
        }
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
        if (horizontalAxisValue != 0)
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
            if (horizontalAxisValue != 0)
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
