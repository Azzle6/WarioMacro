using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NAB2_Civilians : MonoBehaviour
{
    public NAB2_GameManager gameManager;

    public GameObject wayPoint;
    public NAB2_MovePoints movePointsGrid;

    public float moveSpeed, speedModifier;
    private float moveThisFrame;

    public bool difficulty3 = false;

    

    private void Awake()
    {
        //wayPointModifier = Random.Range(-4, 4);
    }
    void Start()
    {
        if (gameManager.gameController.currentDifficulty >= 2)
        {
            ResetWayPoint();
            transform.position = wayPoint.transform.position;

            if (gameManager.gameController.currentDifficulty == 3)
            {
                difficulty3 = true;
            }
        }

    }


    void Update()
    {
        if (difficulty3 == true)
        {
            moveSpeed = gameManager.gameController.currentGameSpeed / speedModifier;
            moveThisFrame = moveSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, wayPoint.transform.position, moveThisFrame);

            if (transform.position == wayPoint.transform.position)
            {
                Debug.Log("change position");
                ResetWayPoint();
            }
        }
    }

    public void ResetWayPoint()
    {
        movePointsGrid.CivilianPoint();
        wayPoint = movePointsGrid.civilianWayPoint;
    }
}
