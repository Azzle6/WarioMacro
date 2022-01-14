using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wayPoints : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position,0.25f);
    }
}
