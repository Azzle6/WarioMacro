using System.Collections;
using UnityEngine;

public class ULC5_MultiBullets : MonoBehaviour {
    private SpriteRenderer sp;
    
    [SerializeField] private Sprite[] starSprites;
    [SerializeField] private Sprite[] bulletSprites;
    
    [SerializeField] private float timeBeforeExplosion;

    void Awake() {
        sp = GetComponent<SpriteRenderer>();
    }
    
    void Start() {
        sp.sprite = starSprites[Random.Range(0, starSprites.Length)];
        transform.localScale = Vector3.zero;
        StartCoroutine(EnlargeCoroutine());
    }

    private IEnumerator EnlargeCoroutine() {
        Vector3 scaleFactor = Vector3.one*2f*Time.deltaTime;
        while (timeBeforeExplosion > 0) {
            transform.localScale += scaleFactor;
            transform.Rotate(transform.forward, 1.5f);
            timeBeforeExplosion -= Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        Debug.Log(transform.localScale);
        
        Explode();
        Destroy(gameObject);
    }
    
    public void Explode() {
        int rdm = Random.Range(1, bulletSprites.Length-1);
        
        SpawnCircle(6,3, bulletSprites[rdm-1],0);
        SpawnCircle(6,4, bulletSprites[rdm],15f);
        SpawnCircle(6,5, bulletSprites[rdm+1],30f);
    }

    private GameObject bullet;
    public void SpawnCircle(int nbBullets, float bulletSpeed, Sprite bulletSprite, float offset) {
        float moveX, moveY;
        
        for (int i = 0; i < nbBullets; i++) {
            bullet = ULC5_Pooler.instance.Pop("Bullet");
            bullet.transform.position = transform.position;

            moveX = Mathf.Sin((360 / nbBullets * i + offset) * Mathf.Deg2Rad);
            moveY = Mathf.Cos((360 / nbBullets * i + offset) * Mathf.Deg2Rad);
            
            bullet.GetComponent<ULC5_Bullet>().InitBullet(bulletSprite, new Vector2(moveX,moveY).normalized, bulletSpeed);
        }
    }
}
