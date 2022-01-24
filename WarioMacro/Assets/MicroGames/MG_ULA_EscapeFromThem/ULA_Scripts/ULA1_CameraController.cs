using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ULA1_CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;
 
    // Update is called once per frame
    void Update()
    {
        if (!ULA1_GameManager.instance.cinematic)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, player.position.z);
        }
    }
}
