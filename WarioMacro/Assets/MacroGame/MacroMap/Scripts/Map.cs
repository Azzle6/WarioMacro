using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Map : MonoBehaviour/*, ITickable*/
{
    [SerializeField] public Node startNode;
    [SerializeField] public Node endNode;
    [SerializeField] public Player player;
    [SerializeField] public GameObject[] arrowPrefabs = new GameObject[3];
    [SerializeField] public int selected = 0;
    private bool isMoving;
    private Queue<Vector3> pathing = new Queue<Vector3>();

    public Node currentNode { get; set; }
    
    private void Awake()
    {
        currentNode = startNode;
        arrowPrefabs.ToList().ForEach(go => go.SetActive(false));
    }
}
