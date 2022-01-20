using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ULA2_CutPart : MonoBehaviour
{
    public float condition;

    public float xOffset;

    public ULA2_ArmScript armScript;

    bool canCut;

    private void Start()
    {
        canCut = true;
    }

    private void Update()
    {
        if(armScript.counter >= condition && canCut)
        {
            canCut = false;
            transform.position = new Vector2(transform.position.x - xOffset, transform.position.y);
        }
    }
}
