using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NAC3_CameraFollowER : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float yMax;
    [SerializeField] private float yMin;
    [SerializeField] private float offset;
    private float newYPos;

    // Update is called once per frame
    void Update()
    {
        newYPos = player.position.y - offset;
        if (newYPos > yMax) newYPos = yMax;
        if (newYPos < yMin) newYPos = yMin;

        transform.position = new Vector3(transform.position.x, newYPos, transform.position.z);
    }
}
