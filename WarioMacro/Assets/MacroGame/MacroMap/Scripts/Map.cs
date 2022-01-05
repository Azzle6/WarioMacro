using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Map : MonoBehaviour/*, ITickable*/
{
    [SerializeField] public Node startNode;
    [SerializeField] public Node endNode;
    
    [SerializeField] public int selected = 0;
    private bool isMoving;
    private Queue<Vector3> pathing = new Queue<Vector3>();

    public Node currentNode { get; set; }
    public Node.Path currentPath { get; set; }
    
    private void Awake()
    {
        currentNode = startNode;
        
    }
}
