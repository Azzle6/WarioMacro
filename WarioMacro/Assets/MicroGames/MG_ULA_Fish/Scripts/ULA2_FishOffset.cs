using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ULA2_FishOffset : MonoBehaviour
{
    public float condition;

    public float xOffset;

    public ULA2_ArmScript armScript;

    private void Update()
    {
        if (armScript.counter == condition)
        {
            transform.position = new Vector2(transform.position.x - xOffset, transform.position.y);
        }
    }
}
