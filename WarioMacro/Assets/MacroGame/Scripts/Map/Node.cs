using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Node : MonoBehaviour
{
    [CanBeNull] public Path[] paths = new Path[4];
    public Animator animator;

    void Start()
    {
        foreach (var path in paths)
        {
            path.wayPoints.Add(path.destination.transform);
        }
    }   
    public static Direction? GetPlayerDirection()
    {
        float horizontalInput = InputManager.GetAxis(ControllerAxis.LEFT_STICK_HORIZONTAL);
        float verticalInput = InputManager.GetAxis(ControllerAxis.LEFT_STICK_VERTICAL);
        
        if (horizontalInput == 0 && verticalInput == 0)
            return null;
        
        float joystickRotation =
            Mathf.Atan2(horizontalInput, verticalInput) * 180 / Mathf.PI;

        if (joystickRotation > -45 && joystickRotation <= 45)
            return Direction.Up;
        if (joystickRotation > 45 && joystickRotation <= 135) 
            return Direction.Right;
        if (joystickRotation > -135 && joystickRotation <= -45) 
            return Direction.Left;
        
        return Direction.Down;
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        // ReSharper disable once PossibleNullReferenceException
        foreach (Path path in paths)
        {
            var points = new List<Vector3>();
            if (path == null) continue;
            if (path.wayPoints.Count == 0)
            {
                if (path.destination != null)
                {
                    points.Add(path.destination.transform.position);
                }
                continue;
            }
            
            foreach(var waypoint in path.wayPoints)
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
        public Direction direction;
        public Node destination;
        public List<Transform> wayPoints = new List<Transform>();
    }

    public enum Direction
    {
        Up, Right, Down, Left
    }
}
