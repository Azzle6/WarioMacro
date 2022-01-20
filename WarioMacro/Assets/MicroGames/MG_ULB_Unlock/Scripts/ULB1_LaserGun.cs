using System.Collections;
using UnityEngine;

public class ULB1_LaserGun : MonoBehaviour
{
    [SerializeField] private bool canShoot = true;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Vector3[] points;
    private Vector3 origin;
    private Vector3 direction;
    private RaycastHit hit;
    [SerializeField] private float maxDistance;
    [SerializeField] private Vector3 point;
    [SerializeField] private float distance;
    [SerializeField] private float noFireDuration = 2;
    [SerializeField] private float fireDuration = 0.5f;

    private void Start()
    {
        points = new Vector3[2];
        origin = transform.position;
        points[0] = Vector3.zero;
        direction = transform.up;
        StartCoroutine(Fire());
    }

    void Update()
    {
        if (canShoot)
        {
            DoRay();
        }

        else
        {
            points[1] = Vector3.zero;
            lineRenderer.SetPositions(points);
        }
    }

    IEnumerator Fire()
    {
        canShoot = false;
        yield return new WaitForSeconds(noFireDuration);
        canShoot = true;
        yield return new WaitForSeconds(fireDuration);
        StartCoroutine(Fire());
    }

    void DoRay()
    {
        if (Physics.Raycast(origin, direction, out hit, maxDistance))
        {
            if (hit.collider.tag == "Player")
            {
                ULB1_GameManager.instance.FinishGame(false);
            }
            point = transform.position-hit.point;
            points[1] = new Vector3(0,point.x+distance,point.z);
            Debug.DrawRay(origin,direction*hit.distance,Color.red);
        }
        else
        {
            Debug.DrawRay(origin,direction,Color.blue*maxDistance,maxDistance);
        }
        lineRenderer.SetPositions(points);
    }
}
