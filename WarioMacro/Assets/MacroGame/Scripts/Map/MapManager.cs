using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private GameObject[] mapGOList;
    private Queue<GameObject> mapGoQueue;

    private Map currentMap;

    public Map LoadNextMap()
    {
        Debug.Log("load next map");
        if (currentMap != null)
        {
            currentMap.Unload();
        }
        
        Debug.Log("Loading + " + mapGoQueue);
        currentMap = mapGoQueue.Dequeue().GetComponent<Map>();
        currentMap.Load();
        
        return currentMap;
    }

    public bool OnLastMap()
    {
        return mapGoQueue.Count == 0;
    }

    private void OnEnable()
    {
        mapGoQueue = new Queue<GameObject>(mapGOList);
        
        Debug.Log("Enable + " + mapGoQueue);
    }
}
