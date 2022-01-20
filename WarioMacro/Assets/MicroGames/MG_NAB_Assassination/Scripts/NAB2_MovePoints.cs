using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NAB2_MovePoints : MonoBehaviour
{
    public GameObject[] wayPoints;
    public GameObject selectedWayPoint;
    public GameObject civilianWayPoint;
    void Awake()
    {
        selectedWayPoint = wayPoints[Random.Range(0, wayPoints.Length)];
        
    }
    private void Start()
    {
     //   civilianWayPoint =
    }

    public void NewWayPoint()
    {
        selectedWayPoint = wayPoints[Random.Range(0, wayPoints.Length)];
    }

    public void CivilianPoint()
    {
        civilianWayPoint = wayPoints[Random.Range(0, wayPoints.Length)];
    }
}
