using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class NOA2_PosLink : MonoBehaviour
{
    [SerializeField] RectTransform target;

    RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private Vector3 offset;
    
    private void Start()
    {
        offset = rectTransform.position - target.position;
    }

    // Update is called once per frame
    void Update()
    {
        rectTransform.position = target.position + offset;
    }
}
