using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NAB2_Target : MonoBehaviour
{
    public NAB2_GameManager gameManager;

    public GameObject wayPoint;
    public NAB2_MovePoints movePointsGrid;

    public float moveSpeed, speedModifier;
    public float moveThisFrame;

    public bool difficulty1 = false, difficulty2 = false, difficulty3 = false;
    public NAB2_Shot shot;
    void Start()
    {
        wayPoint = movePointsGrid.selectedWayPoint;
        transform.position = wayPoint.transform.position;

        if (gameManager.gameController.currentDifficulty == 1)
        {
            difficulty1 = true;
        }

        if (gameManager.gameController.currentDifficulty == 2)
        {
            difficulty2 = true;
        }

        if (gameManager.gameController.currentDifficulty == 3)
        {
            difficulty3 = true;
        }

    }

    
    void Update() 
    {
        if(difficulty2 == true || difficulty3 == true && shot.win == false)
        {
            moveSpeed = gameManager.gameController.currentGameSpeed / speedModifier;
            moveThisFrame = moveSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, wayPoint.transform.position, moveThisFrame);

            if(transform.position == wayPoint.transform.position)
            {
                Debug.Log("change position");
                ResetWayPoint();
            }
        }
    }

    public void ResetWayPoint()
    {
        movePointsGrid.NewWayPoint();
        wayPoint = movePointsGrid.selectedWayPoint;
    }
}
