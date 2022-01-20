using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeConnectionRenderer : MonoBehaviour
{
    public GameObject prefab;
    private GameObject temp;
    
    public void CreatePathRenderer(Node.Path path)
    {
        List<Vector3> points = new List<Vector3>();
        points.Add(transform.position);
        foreach (var wayPoint in path.wayPoints)
        {
            points.Add(wayPoint.position);
        }
        points.Add(path.destination.transform.position);
        Debug.Log(points);
        if (points.Count > 1)
        {
            for (int i = 0; i < points.Count - 1; i++)
            {
                Vector2 vec;
                vec.x = points[i].x + (points[i+1].x - points[i].x )/2;
                vec.y = points[i].y + (points[i+1].y - points[i].y )/2;
                temp = Instantiate(prefab, vec, Quaternion.identity);
                temp.GetComponent<SpriteRenderer>().size = new Vector2((points[i+1]-points[i]).magnitude*1.62f, 0.62f);
                temp.transform.right = points[i + 1] - points[i];
                temp.transform.parent = path.pathRenderer.transform;
            }
        }
        else
        {
            Vector2 vec;
            vec.x = transform.position.x + (path.destination.transform.position.x - transform.position.x )/2;
            vec.y = transform.position.y + (path.destination.transform.position.y )/2;
            temp = Instantiate(prefab, vec, Quaternion.identity);
            temp.GetComponent<SpriteRenderer>().size = new Vector2((path.destination.transform.position-transform.position).magnitude*1.62f, 0.62f);
            temp.transform.right = path.destination.transform.position - transform.position;
            temp.transform.parent = path.pathRenderer.transform;
        }
            
        points.Clear();
        
        
    }
}
