using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeConnectionRenderer : MonoBehaviour
{
    public GameObject prefab;
    private GameObject temp;
    private List<Vector3> points;

    public void CreatePathRenderer(Node.Path path)
    {
        ClearPath();
        points = new List<Vector3>();
        points.Add(transform.position);
        foreach (var wayPoint in path.wayPoints)
        {
            points.Add(wayPoint.position);
        }
        points.Add(path.destination.transform.position);
        //Debug.Log(points);
        if (points.Count > 1)
        {
            for (int i = 0; i < points.Count - 1; i++)
            {

                temp = Pooler.instance.Pop("path");
                temp.transform.position = MidPoint(points[i], points[i + 1]);
                temp.GetComponent<SpriteRenderer>().size = new Vector2((points[i+1]-points[i]).magnitude*1.62f, 0.62f);
                temp.transform.GetChild(0).GetComponent<SpriteRenderer>().size = new Vector2((points[i+1]-points[i]).magnitude*1.62f, 0.62f);
                temp.transform.right = points[i + 1] - points[i];
                temp.transform.parent = transform;
            }
        }
        else
        {
            temp = Pooler.instance.Pop("path");
            temp.transform.position = MidPoint(transform.position, path.destination.transform.position);
            temp.GetComponent<SpriteRenderer>().size = new Vector2((path.destination.transform.position-transform.position).magnitude*1.62f, 0.62f);
            temp.transform.GetChild(0).GetComponent<SpriteRenderer>().size = new Vector2((path.destination.transform.position-transform.position).magnitude*1.62f, 0.62f);
            temp.transform.right = path.destination.transform.position - transform.position;
            temp.transform.parent = transform;
        }
        points.Clear();
    }
    

    public void ClearPath()
    {
        for (int i = transform.childCount-1;i>-1;i--)
        {
            Pooler.instance.DePop("path",transform.GetChild(i).gameObject);
        }
    }
    
    Vector2 MidPoint(Vector2 vec1,Vector2 vec2)
    {
        Vector2 vec;
        vec.x = vec1.x + (vec2.x  - vec1.x)/2;
        vec.y = vec1.y + (vec2.y - vec1.y)/2;
        return vec;
    }
}
