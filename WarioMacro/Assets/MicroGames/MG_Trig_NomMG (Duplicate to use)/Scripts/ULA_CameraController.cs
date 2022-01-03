using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ULA_CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;
 
    // Update is called once per frame
    void Update()
    {
        if (!ULA_GameManager.instance.init)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, player.position.z);
        }
    }
}
