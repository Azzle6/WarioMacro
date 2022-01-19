using System.Collections.Generic;
using UnityEngine;

public class NAA2_Surveillance_CameraMovement : MonoBehaviour, ITickable
{
    [SerializeField]
    NAA2_Surveillance_MicroGameController mcg;
    [SerializeField]
    float bpm;
    public bool change, stop;
    bool movementStarted;

    [SerializeField]
    float waitTime;

    public float camSpeed;
    bool exited;
    float elapsed;

    public List<Waypoint> waypoints = new List<Waypoint>();
    public List<Waypoint> transitionWaypoints = new List<Waypoint>();
    public List<Waypoint> chosenWaypoints = new List<Waypoint>();

    [SerializeField]
    List<Waypoint> route = new List<Waypoint>();

    [SerializeField]
    Waypoint currentWaypoint, targetWaypoint;

    [SerializeField]
    NAA2_Surveillance_CameraDetect cd;

    [SerializeField]
    GameObject upWall, downWall, rightWall, leftWall, targetSprite;



    [System.Serializable]
    public class Waypoint
    {
        public GameObject waypoint;
        public int angle;
    }

    // Start is called before the first frame update
    void Start()
    {
        bpm = mcg.gc.currentGameSpeed;
        SelectWaypoints();

        GameManager.Register();
        GameController.Init(this);
    }

    void SelectWaypoints()
    {

        float distance;
        for (int i = 0; i < 8; i++)
        {
            chosenWaypoints[i] = waypoints[Random.Range(0, waypoints.Count)];
            if (i != 0)
            {
                distance = Vector3.Distance(chosenWaypoints[i].waypoint.transform.position, chosenWaypoints[i - 1].waypoint.transform.position);
                if (distance <= 2 || distance > 15)
                {
                    i--;
                }

                if (i == 1)
                {
                    if (chosenWaypoints[i] == chosenWaypoints[0])
                    {
                        i--;
                    }

                }
                else if (i > 2)
                {
                    if (chosenWaypoints[i] == chosenWaypoints[i - 1] || chosenWaypoints[i] == chosenWaypoints[i - 2])
                    {
                        i--;
                    }
                }
            }
        }
        targetWaypoint = chosenWaypoints[0];
        targetSprite.transform.position = targetWaypoint.waypoint.transform.position;
    }

    void DefineRoute()
    {
        route.Clear();
        if (currentWaypoint.waypoint.transform.position.x == targetWaypoint.waypoint.transform.position.x || currentWaypoint.waypoint.transform.position.y == targetWaypoint.waypoint.transform.position.y)
        {
            route.Add(targetWaypoint);
            return;
        }
        else
        {
            Waypoint transitionWay = null;
            foreach (Waypoint way in transitionWaypoints)
            {
                if ((way.waypoint.transform.position.x == currentWaypoint.waypoint.transform.position.x || way.waypoint.transform.position.y == currentWaypoint.waypoint.transform.position.y) && (way.waypoint.transform.position.x == targetWaypoint.waypoint.transform.position.x || way.waypoint.transform.position.y == targetWaypoint.waypoint.transform.position.y))
                {
                    transitionWay = way;
                    route.Add(way);
                    route.Add(targetWaypoint);
                    return;
                }
            }

            if (transitionWay == null)
            {
                foreach (Waypoint way in transitionWaypoints)
                {
                    if (way.waypoint.transform.position.x == currentWaypoint.waypoint.transform.position.x || way.waypoint.transform.position.y == currentWaypoint.waypoint.transform.position.y)
                    {
                        foreach (Waypoint w in transitionWaypoints)
                        {

                            if ((w.waypoint.transform.position.x == way.waypoint.transform.position.x || w.waypoint.transform.position.y == way.waypoint.transform.position.y) && w != way)
                            {
                                route.Add(way);
                                route.Add(w);
                                route.Add(targetWaypoint);
                                return;
                            }
                        }
                    }
                }
            }

        }
    }

    public void OnTick()
    {
        movementStarted = true;
        if (GameController.currentTick == 1)
        {
            targetWaypoint = chosenWaypoints[0];
            DefineRoute();
        }
        else
        {
            if (transform.position == targetWaypoint.waypoint.transform.position)
            {
                if (GameController.currentTick > 1)
                {
                    currentWaypoint = targetWaypoint;
                }
                targetWaypoint = chosenWaypoints[GameController.currentTick - 1];
                targetSprite.transform.position = targetWaypoint.waypoint.transform.position;
                DefineRoute();

            }

        }
        if (GameController.currentTick == 5)
        {
            movementStarted = false;
            stop = true;
        }


    }

    void FollowRoute()
    {
        if (route.Count == 0 ||stop) return;
        Vector3 movement = (Vector3.MoveTowards(transform.position, route[0].waypoint.transform.position, (GameController.gameBPM * Time.deltaTime) / 5));
        if (!movementStarted) movement = transform.position;
        transform.position = movement;

        if (transform.position == route[0].waypoint.transform.position)
        {
            route.RemoveAt(0);
        }
    }   


    // Update is called once per frame
    void Update()
    {

        if (mcg.gameStarted && !movementStarted)
        {

        }

        if (movementStarted)
        {
            if (transform.position != targetWaypoint.waypoint.transform.position)
            {
                FollowRoute();
            }
            if (targetWaypoint.waypoint.transform.position == transform.position)
            {
                movementStarted = false;
            }
        }


        if (exited)
        {
            elapsed -= Time.deltaTime;
            if (elapsed <= 0)
            {
                transform.position = currentWaypoint.waypoint.transform.position;
                targetWaypoint = currentWaypoint;
                DefineRoute();
            }

        }
        else
        {
            elapsed = 0.2f;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.gameObject == upWall)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            exited = false;
        }
        if (collision.gameObject == downWall)
        {
            transform.rotation = Quaternion.Euler(0, 0, 180);
            exited = false;
        }
        if (collision.gameObject == leftWall)
        {
            transform.rotation = Quaternion.Euler(0, 0, 90);
            exited = false;
        }
        if (collision.gameObject == rightWall)
        {
            transform.rotation = Quaternion.Euler(0, 0, 270);
            exited = false;
        }

    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        exited = true;
    }
}
