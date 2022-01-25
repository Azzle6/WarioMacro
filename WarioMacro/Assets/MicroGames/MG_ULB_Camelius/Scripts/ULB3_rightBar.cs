using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ULB3_rightBar : MonoBehaviour
{
  public bool rightBarTouched; 
    private void OnTriggerEnter(Collider other)
    {
        rightBarTouched = true;
    }
    private void OnTriggerExit(Collider other)
    {
        rightBarTouched = false;
    }
}
