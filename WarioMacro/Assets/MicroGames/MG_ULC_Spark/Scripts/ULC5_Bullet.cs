using System;
using UnityEngine;

public class ULC5_Bullet : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sp;

    private Vector2 moveAxis;
    private float speed;

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        sp = GetComponent<SpriteRenderer>();
    }
    
    void FixedUpdate() {
        rb.velocity = moveAxis * speed;
    }

    public void InitBullet(Sprite sprite, Vector2 moveAxis, float speed) {
        sp.sprite = sprite;
        this.moveAxis = moveAxis;
        this.speed = speed;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.GetComponent<ULC5_BulletSpawner>()) ULC5_Pooler.instance.Depop("Bullet",gameObject);
    }

    private void OnDisable() {
        rb.velocity = Vector2.zero;;
        sp.sprite = null;
        moveAxis = Vector2.zero;
        transform.position = Vector3.zero;
    }
}
