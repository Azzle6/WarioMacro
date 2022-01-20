using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

// ReSharper disable once CheckNamespace
public class Node : MonoBehaviour
{
    public Animator animator;
    public Path[] paths = new Path[4];
    public NodeConnectionRenderer pathRenderer;
    public MoveDirection? SelectedDirection = null;

    
    
    private void Start()
    {
        foreach (Path path in paths.Where(p => p != null))
        {
            path.wayPoints.Add(path.destination.transform);
            path.pathRenderer = Instantiate(new GameObject(), transform.position, Quaternion.identity, transform);
            pathRenderer.CreatePathRenderer(path);
            //path.pathRenderer.SetActive(false);
        }
    }

    private void Update()
    {
        foreach (Path path in paths.Where(p => p != null))
        {
            path.pathRenderer.SetActive(path.direction == SelectedDirection);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        
        foreach (Path path in paths.Where(p => p != null))
        {
            var points = new List<Vector3>();
            
            if (path.wayPoints == null)
            {
                if (path.destination != null)
                {
                    points.Add(path.destination.transform.position);
                }
                continue;
            }
            
            foreach(Transform waypoint in path.wayPoints)
            {
                points.Add(waypoint.position);
            }
            if (path.destination != null)
            {
                points.Add(path.destination.transform.position);
            }
            for (int i = 0; i < points.Count; i++)
            {
                Vector3 p0 = i > 0 ? points[i - 1] : transform.position;
                Vector3 p1 = points[i];
                Gizmos.DrawLine(p0, p1);
            }

        }
    }
    
    [Serializable]
    public class Path
    {
        public MoveDirection direction;
        public Node destination;
        public GameObject pathRenderer;
        public List<Transform> wayPoints = new List<Transform>();
    }
}
