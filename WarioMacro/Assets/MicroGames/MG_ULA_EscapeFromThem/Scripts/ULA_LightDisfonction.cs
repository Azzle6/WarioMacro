using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ULA_LightDisfonction : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshR;
    [SerializeField] private Material matePasHDR;
    [SerializeField] private Material mateHDR;
    private int random;
    private float timeRandom;

    // Update is called once per frame
    void Start()
    {
        StartCoroutine(CoroutineChangeColor());
    }

    IEnumerator CoroutineChangeColor()
    {
        timeRandom = Random.Range(0.3f, 0.6f);
        yield return new WaitForSeconds(timeRandom);
        
        random = Random.Range(0, 11);
        if (random > 2)
        {
            meshR.material = matePasHDR;
        }
        else
        {
            meshR.material = mateHDR;
        }
        
        StartCoroutine(CoroutineChangeColor());
    }
}
