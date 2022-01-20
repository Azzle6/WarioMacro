using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NAC3_ParallaxER : MonoBehaviour
{
    [SerializeField] private Transform background;
    [SerializeField] private Transform camera;
    [SerializeField] private float parallaxSpeed;
    private Vector3 firstBackgroundPos;
    private Vector3 firstCameraPos;

    private void Start()
    {
        firstBackgroundPos = background.position;
        firstCameraPos = camera.position;
    }

    private void Update()
    {
        background.position = firstBackgroundPos + new Vector3((firstCameraPos.x - camera.position.x) / background.position.z * parallaxSpeed, (firstCameraPos.y - camera.position.y) / background.position.z * parallaxSpeed, 0);
    }
}
