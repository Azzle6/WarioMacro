using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NOA2_UpPartSaw : MonoBehaviour
{
    [SerializeField] private Transform mainSaw;

    [SerializeField] private float xValue;
    [SerializeField] private float yValue;
    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(mainSaw.position.x + xValue, mainSaw.position.y + yValue, mainSaw.position.z);
    }
}
