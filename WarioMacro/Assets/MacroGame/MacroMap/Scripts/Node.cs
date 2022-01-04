using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using System.Linq;

public class Node : MonoBehaviour
{
    [CanBeNull] public Path[] paths = new Path[4];
    public Animator animator;

    public Path GetNodeFromInput(ControllerKey key)
    {
        switch (key)
        {
            case ControllerKey.DPAD_UP: return paths[0];
            case ControllerKey.DPAD_LEFT: return paths[1];
            case ControllerKey.DPAD_DOWN: return paths[2];
            case ControllerKey.DPAD_RIGHT: return paths[3];
            default: return null;
        }
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
                foreach (Transform step in path.steps)
                {
                    if (step == null) continue;
                    points.Add(step.position);
                }
            }

            for (int i = 0; i < points.Count; i++)
            {
                var p0 = i > 0 ? points[i - 1] : transform.position;
                var p1 = points[i];
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
    }

    public enum Direction
    {
        Up, Right, Down, Left
    }
}
