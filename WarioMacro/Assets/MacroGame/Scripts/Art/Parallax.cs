using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public float parmultiplier_x = 0.0f;
    public float parmultiplier_y = 0.0f;
    public bool parhorizontalOnly = true;

    private Transform cameraTransform;

    public Vector3 startCameraPos;
    public Vector3 startPos;

    void Start()
    {
        cameraTransform = Camera.main.transform;
        startCameraPos = cameraTransform.position;
        startPos = transform.position;
    }


    private void LateUpdate()
    {
        var position = startPos;
        if (parhorizontalOnly)
            position.x += parmultiplier_x * (cameraTransform.position.x - startCameraPos.x);
        else
            position.x += parmultiplier_x * (cameraTransform.position.x - startCameraPos.x);
        position.y += parmultiplier_y * (cameraTransform.position.y - startCameraPos.y);

        transform.position = position;
    }
}
