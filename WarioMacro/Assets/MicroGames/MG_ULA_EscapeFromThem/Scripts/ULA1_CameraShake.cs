using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ULA1_CameraShake : MonoBehaviour
{
    public static ULA1_CameraShake instance;
    [SerializeField] private float shakeAmount = 0.1f;
    private bool isShaking;
    private int random;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (isShaking)
        {
            random = Random.Range(-2, 3);
            //transform.localPosition = new Vector3(transform.localPosition.x+random, transform.localPosition.y + random) * shakeAmount;
        }
    }

    public void Shake(float f)
    {
        isShaking = true;
        StartCoroutine(EndShakingCoroutine(f));
    }

    IEnumerator EndShakingCoroutine(float f)
    {
        yield return new WaitForSeconds(f);
        isShaking = false;
    }
}
