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
            /*if(cutterController.canDeleteCorners == true)
            {
                other.GetComponent<BoxCollider2D>().enabled = false;
                cutterController.canDeleteCorners = false;
            }*/
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("CornerBill") && cutterController.canDeleteCorners == true)
        {
            collision.GetComponent<BoxCollider2D>().enabled = false;
            cutterController.canDeleteCorners = false;
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
