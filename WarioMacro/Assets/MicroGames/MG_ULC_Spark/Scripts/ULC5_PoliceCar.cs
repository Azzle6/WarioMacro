using System.Collections;
using UnityEngine;

public class ULC5_PoliceCar : MonoBehaviour {

    private Rigidbody2D rb;
    private SpriteRenderer sp;

    [SerializeField] private Sprite[] policeCarSprites;
    
    [SerializeField] private float moveSpeed;

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        sp = GetComponent<SpriteRenderer>();
        sp.sprite = policeCarSprites[Random.Range(1, policeCarSprites.Length)];
    }

    void Start() {
        rb.velocity = -transform.up * moveSpeed;
        StartCoroutine(MoveCoroutine());
    }

    private IEnumerator MoveCoroutine() {
        yield return new WaitForSeconds(0.1f);
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(1f);
        rb.velocity = -transform.up * moveSpeed;
    }
}
