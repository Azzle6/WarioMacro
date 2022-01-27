using UnityEngine;
using DG.Tweening;

public class CameraFollowPlayer : MonoBehaviour
{
    [SerializeField] private Camera mainCam;
    [SerializeField] private GameObject player;
    [SerializeField] private Vector3 backgroundScale;
    private float cameraHeight;
    private float cameraWidth;

    private void Start()
    {
        cameraHeight = 2f * mainCam.orthographicSize;
        cameraWidth = cameraHeight * mainCam.aspect;
    }

    private void Update()
    {
        Vector3 position = player.transform.position;
        mainCam.transform.position = new Vector3(
            Mathf.Clamp(
                position.x,
                -(backgroundScale.x/2 - cameraWidth/2),
                (backgroundScale.x/2 - cameraWidth/2)
            ), 
            Mathf.Clamp(
                position.y,
                -(backgroundScale.y/2 - cameraHeight/2),
                (backgroundScale.y/2 - cameraHeight/2)
            ), 
            -10);
    }
}
