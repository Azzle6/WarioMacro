using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using System.Linq;
// ReSharper disable PossibleNullReferenceException

public class Node : MonoBehaviour
{
    [CanBeNull] public Path[] paths = new Path[4];
    public Animator animator;

    public Path GetNodeFromInput(ControllerKey key)
    {
        return key switch
        {
            ControllerKey.DPAD_UP => paths[0],
            ControllerKey.DPAD_LEFT => paths[1],
            ControllerKey.DPAD_DOWN => paths[2],
            ControllerKey.DPAD_RIGHT => paths[3],
            _ => null
        };
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        foreach (Path path in paths)
        {
            if (path == null) continue;
            var points = new List<Vector3>();
            if (path.destination != null)
            {
                points.Add(path.destination.transform.position);
            }

            if (path.steps != null)
            {
                points.AddRange(from step in path.steps where step != null select step.position);
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
        [CanBeNull] public Node destination;
        [CanBeNull] public Transform[] steps;

        // public Vector3[] GetPositions()
        // {
        //     var points = new List<Vector3>();
        //     foreach(Transform step in steps)
        //         points.Add(step.position);
        //     points.Add(destination.transform.position);
        //     return points.ToArray();
        // }
    }

    public enum Direction
    {
        Up, Right, Down, Left
    }
}
