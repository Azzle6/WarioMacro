using UnityEngine;

public class ULC5_Player : MonoBehaviour {
    
    private Animator anim;
    private Rigidbody2D rb;

    [SerializeField] private float speed;
    private Vector2 moveAxis;

    private bool isDead;
    
    void Awake() {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() {
        if (!ULC5_GameManager.instance.inGame) moveAxis = Vector2.zero;
        else moveAxis = new Vector2(InputManager.GetAxis(ControllerAxis.LEFT_STICK_HORIZONTAL), InputManager.GetAxis(ControllerAxis.LEFT_STICK_VERTICAL));
        MoveAnim();
    }
    
    void FixedUpdate() {
        if (!isDead) rb.velocity = moveAxis.normalized * speed / Time.timeScale;

        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, -8.2f, 8.2f),
            Mathf.Clamp(transform.position.y, -4.2f, 4.2f),
            transform.position.z);
    }

    private void MoveAnim() {
        if (moveAxis.x < 0) anim.Play("Left");
        else if (moveAxis.x > 0) anim.Play("Right");
        else anim.Play("Idle");
    }
    
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.GetComponent<ULC5_DeathBeam>()) {
            Vector2 hitPoint = other.ClosestPoint(transform.position);
            ULC5_DeathBeam.instance.SummonBeam(hitPoint,-rb.velocity);
            gameObject.SetActive(false);
        }
        else {
            isDead = true;
            ULC5_GameManager.instance.EndGame(false);
            rb.angularVelocity = 720;
            
            Vector2 deathForce = new Vector2(-rb.velocity.x, Random.Range(-1f, 1f)).normalized * 15;
            rb.velocity = Vector2.zero;
            rb.AddForce(deathForce,ForceMode2D.Impulse);
        }
    }
}
