using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NAA2_Surveillance_RoomConfig : MonoBehaviour
{
    [SerializeField]
    NAA2_Surveillance_MicroGameController mgc;




    [System.Serializable]
    public class Obstacle
    {
        [SerializeField]
        public GameObject obstacle;
        [SerializeField]
        public Vector2 position;
    }

    [System.Serializable]
    public class Room
    {
        [SerializeField]
        public List<Obstacle> obstacles = new List<Obstacle>();

    }


    public List<Room> ezRooms = new List<Room>();
    public List<Room> medRooms = new List<Room>();
    public List<Room> hardRooms = new List<Room>();

    Room currentRoom;

    List<GameObject> props = new List<GameObject>();
    int randomizer;
    private void Start()
    {
        if(mgc.gc.currentDifficulty == 1)
        {
            randomizer = Random.Range(0, ezRooms.Count);
            currentRoom = ezRooms[randomizer];

            foreach(Obstacle obs in currentRoom.obstacles)
            {
                Instantiate(obs.obstacle, transform.position + new Vector3(obs.position.x, obs.position.y, 0), Quaternion.identity, transform);
            }
        }

        if (mgc.gc.currentDifficulty == 2)
        {
            randomizer = Random.Range(0, medRooms.Count);
            currentRoom = medRooms[randomizer];

            foreach (Obstacle obs in currentRoom.obstacles)
            {
                Instantiate(obs.obstacle, transform.position + new Vector3(obs.position.x, obs.position.y, 0), Quaternion.identity, transform);
            }
        }

        if (mgc.gc.currentDifficulty == 3)
        {
            randomizer = Random.Range(0, hardRooms.Count);
            currentRoom = hardRooms[randomizer];

            foreach (Obstacle obs in currentRoom.obstacles)
            {
                Instantiate(obs.obstacle, transform.position + new Vector3(obs.position.x, obs.position.y, 0), Quaternion.identity, transform);
            }
        }
    }

}
