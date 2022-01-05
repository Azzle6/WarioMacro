using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class NOB_PAP_PlankHit : MonoBehaviour
{
    private NOB_PAP_PlankManager plankManager;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private AudioClip plankHitAudio;
    [SerializeField] private AudioClip plankBreakAudio;
    [SerializeField] private float timeBeforeDisable;
    [SerializeField] private float shakeForce;
    [SerializeField] private float popForce;
    [SerializeField] private float torqueForce;
    private Vector3 beforeShakePos;
    public int remainingHits;

    private void Start()
    {
        plankManager = transform.parent.GetComponent<NOB_PAP_PlankManager>();
        remainingHits = Random.Range(2, 4);
        beforeShakePos = transform.position;
    }

    public void GetHit()
    {
        remainingHits--;
        if (remainingHits == 0)
        {
            PopOut();
            AudioManager.PlaySound(plankBreakAudio);
        }
        else
        {
            Shake();
            AudioManager.PlaySound(plankHitAudio);
        }
    }

    private void PopOut()
    {
        plankManager.DistachPlanch();
        boxCollider.enabled = false;
        rb.isKinematic = false;
        rb.AddForce(new Vector2(Random.Range(-0.5f,0.5f), Random.Range(0.5f, 1f)) * popForce);
        rb.AddTorque(Random.Range(-torqueForce,torqueForce));
        StartCoroutine(DisablePlank());
    }

    IEnumerator DisablePlank()
    {
        yield return new WaitForSeconds(timeBeforeDisable);
        gameObject.SetActive(false);
    }
    private void Shake()
    {
        StartCoroutine(ShakeCoroutine());
    }
    
    IEnumerator ShakeCoroutine()
    {
        for (int i = 0; i < 5; i++)
        {
            transform.position = beforeShakePos;
            transform.position = new Vector3(beforeShakePos.x + Random.Range(-shakeForce, shakeForce) * (1f / remainingHits), beforeShakePos.y + Random.Range(-shakeForce, shakeForce) * (1f / remainingHits),
                        transform.position.z);
            yield return new WaitForSeconds(0.01f);
        }
        transform.position = beforeShakePos;
    }
}
