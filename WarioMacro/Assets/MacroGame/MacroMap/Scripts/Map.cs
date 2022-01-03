using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Map : MonoBehaviour/*, ITickable*/
{
    [SerializeField] public Node startNode;
    [SerializeField] public Node endNode;
    [SerializeField] public Node currentNode;
    [SerializeField] public Player player;
    [SerializeField] public GameObject arrowPrefab;
    [SerializeField] public int selected = 0;
    private bool isMoving;
    private Queue<Vector3> pathing = new Queue<Vector3>();

    private void Awake()
    {
        currentNode = startNode;
        arrowPrefab.SetActive(false);
    }

   /* private void Start()
    {
        GameManager.Register();
        GameController.Init(this);
        currentNode = startNode;
        player.transform.position = startNode.transform.position;
    }

    void Update()
    {
        if (InputManager.GetKeyDown(ControllerKey.A))
        {
            StartMoving();
            arrowPrefab.SetActive(false);
        }
        if (player.transform.position == currentNode.paths[selected].destination.transform.position)
        {
            isMoving = false;
            currentNode = currentNode.paths[selected].destination;
            player.StopMoving();
        }

        if (!isMoving)
        {
            SelectPath();
        }
    }

    
    public void OnTick()
    {
        if (isMoving)
        {
            player.Move(pathing.Dequeue());
        }

        
        
    }

    void StartMoving()
    {
        if (currentNode.paths.Any())
        {
            isMoving = true;
            foreach (var tr in currentNode.paths[selected].steps)
            {
                pathing.Enqueue(tr.position);
            }

            pathing.Enqueue(currentNode.paths[selected].destination.transform.position);

        }
    }

    private bool axisUse;
    void SelectPath()
    {
        if (!isMoving)
        {
            arrowPrefab.SetActive(true);

            if (InputManager.GetAxisRaw(ControllerAxis.LEFT_STICK_VERTICAL) != 0)
            {
                if (axisUse == false)
                {
                    if (InputManager.GetAxisRaw(ControllerAxis.LEFT_STICK_VERTICAL) == 1)
                    {
                        Debug.Log("UP");
                        selected--;
                
                        if (selected < 0)
                        {
                            selected = currentNode.paths.Length - 1;
                        }
                        axisUse = true;
                    }
                    if (InputManager.GetAxisRaw(ControllerAxis.LEFT_STICK_VERTICAL) == -1)
                    {
                        Debug.Log("DOWN");
                        selected++;
                        if (selected >= currentNode.paths.Length)
                        {
                            selected = 0;
                        }
                        axisUse = true;
                    }
                    
                }
            }

            if (InputManager.GetAxisRaw(ControllerAxis.LEFT_STICK_VERTICAL) == 0)
            {
                axisUse = false;
            }
            
            arrowPrefab.transform.parent = currentNode.paths[selected].destination.transform;
            arrowPrefab.transform.localPosition = new Vector2(0, 1);
        }
    }*/
}
