using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ULC2_Dolla_CornerScript : MonoBehaviour
{
    [SerializeField] private ULC2_Dolla_CutterController cutterController;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("CornerBill"))
        {
            cutterController.cornersReached++;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("CornerBill"))
        {
            cutterController.cornersReached--;
        }
    }
}
