using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static int floor { get; private set; } = -1;
    
    [SerializeField] private Player player;
    [SerializeField] private GameObject[] mapGOList;
    private Queue<GameObject> mapGoQueue;

    private Map currentMap;

    public Map LoadNextMap()
    {
        if (currentMap != null)
        {
            currentMap.Unload();
        }
        
        currentMap = mapGoQueue.Dequeue().GetComponent<Map>();
        currentMap.Load();
        floor++;
        
        return currentMap;
    }

    public bool OnLastMap()
    {
        return mapGoQueue.Count == 0;
    }

    private void OnEnable()
    {
        mapGoQueue = new Queue<GameObject>(mapGOList);
    }
}
