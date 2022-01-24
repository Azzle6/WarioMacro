using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NOB1_SpawnerManager : MonoBehaviour
{
    int randomObject;
    int randomSpawner;

    public float timeBeforeSpawn = 1.0f;
    float currentTime = 0f;

    List<GameObject> spawners;

    public GameObject spawner1;
    public GameObject spawner2;
    public GameObject spawner3;

    public GameObject billet;
    public GameObject bomb;

    // Start is called before the first frame update
    void Start()
    {
        spawners = new List<GameObject>();
        AddSpawnersInList();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTime+timeBeforeSpawn < Time.fixedTime)
        {
            randomSpawner = Random.Range(0, 3);
            randomObject = Random.Range(1, 5);
            if (randomObject == 4)
            {
                Instantiate(bomb, spawners[randomSpawner].transform.position, spawners[randomSpawner].transform.rotation);
            } else
            {
                Instantiate(billet, spawners[randomSpawner].transform.position, spawners[randomSpawner].transform.rotation);
            }

            currentTime = Time.fixedTime;
        }
    }


    void AddSpawnersInList()
    {
        spawners.Add(spawner1);
        spawners.Add(spawner2);
        spawners.Add(spawner3);
    }


}
