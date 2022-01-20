using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NAC3_Laser : MonoBehaviour
{
    [SerializeField] private NAC3_EnRappelController erController;

    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private float laserLength;

    [SerializeField] private bool isRotating;
    private float startAngle;
    [SerializeField] private float angle;
    [SerializeField] private float rotationTime;
    [SerializeField] private bool flipRotation;
    private float countdownRotation;

    [SerializeField] private bool isMoving;
    private Vector3 startPosition;
    [SerializeField] private Vector2 movePosition;
    [SerializeField] private float moveTime;
    //[SerializeField] private bool flipMovement;
    private float countdownMove;

    delegate void TransformOperation();
    TransformOperation transformOperation;

    [SerializeField] private AudioClip laserSound;

    private void Start()
    {
        AudioManager.PlaySound(laserSound, 0.2f);
        startAngle = transform.eulerAngles.z;
        startPosition = transform.position;

        countdownRotation = 0;
        countdownMove = 0;

        if (isMoving) transformOperation += MoveLaser;
        if (isRotating) transformOperation += RotateLaser;
    }

    RaycastHit hit;

    private void Update()
    {
        if (Physics.Raycast(transform.position, transform.right * laserLength, out hit, laserLength))
        {
            lineRenderer.SetPosition(1, new Vector3(transform.InverseTransformPoint(hit.point).x+ 0.2f, 0, 0));
            if (hit.collider.transform.CompareTag("Player")) 
            {
                erController.CheckEndGame(false);
            }
        }
        else 
        {
            lineRenderer.SetPosition(1, new Vector3(laserLength, 0, 0));
        }
        Debug.DrawRay(transform.position, transform.right, Color.blue, 1);

        if (transformOperation != null) 
        {
            transformOperation();
            countdownRotation += Time.deltaTime;
            countdownMove += Time.deltaTime;
        }
    }

    private float newAngle;

    private void RotateLaser() 
    {
        if (countdownRotation >= rotationTime) 
        {
            angle = -angle;
            countdownRotation = 0;
        }

        newAngle = Mathf.LerpAngle(flipRotation ? -angle : angle, flipRotation ? angle : -angle, countdownRotation/rotationTime);
        transform.eulerAngles = new Vector3(0, 0, startAngle + newAngle);
    }

    private Vector3 newPosition;

    private void MoveLaser() 
    {
        if (countdownMove >= moveTime) 
        {
            movePosition = new Vector2(-movePosition.x, -movePosition.y);
            countdownMove = 0;
        }

        newPosition = new Vector3(Mathf.Lerp(-movePosition.x, movePosition.x, countdownMove/moveTime), Mathf.Lerp(-movePosition.y, movePosition.y, countdownMove/moveTime), 0);
        transform.position = startPosition + newPosition;
    }
}