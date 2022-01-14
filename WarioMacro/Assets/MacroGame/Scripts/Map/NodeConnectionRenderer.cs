using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeConnectionRenderer : MonoBehaviour
{
    public Node node;
    public LineRenderer lineRenderer;

    void Start()
    {
        UpdateLineRenderer();
    }

    void OnDrawGizmos()
    {
        UpdateLineRenderer();
    }
    
    [ContextMenu("UpdateLineRenderer")]
    private void UpdateLineRenderer()
    {
        if (node == null) return;
        if ( lineRenderer == null) return;
        List<Vector3> points = new List<Vector3>();
        foreach (var path in node.paths)
        {
            if (path.destination == null) continue;
            if (path.wayPoints.Count == 0) continue;
            points.Add(transform.position);
            foreach (var wayPoint in path.wayPoints)
            {
                points.Add(wayPoint.position);
            }
            points.Add(path.destination.transform.position);
        }

        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPositions(points.ToArray());
    }
}
