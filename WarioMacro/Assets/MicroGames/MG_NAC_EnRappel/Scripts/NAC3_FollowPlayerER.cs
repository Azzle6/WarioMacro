using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NAC3_FollowPlayerER : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private LineRenderer line;

    void Update()
    {
        transform.position = new Vector3(player.position.x, transform.position.y, transform.position.z);
        line.SetPosition(0, transform.position);
        line.SetPosition(1, player.position);
    }
}
