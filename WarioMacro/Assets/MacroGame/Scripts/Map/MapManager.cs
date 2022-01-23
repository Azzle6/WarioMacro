using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GameTypes;
using UnityEngine;
using Random = System.Random;

// ReSharper disable once CheckNamespace
public class MapManager : MonoBehaviour
{
    public static int phase = 0;
    public static int floor { get; private set; } = -1;
    public Dictionary<int, float> typePercentages = new Dictionary<int, float>();

    [SerializeField] private Map recruitmentMap;
    [SerializeField] private int mapsPerGame = 5;
    [SerializeField] private GameObject[] mapGOList;
    private Queue<GameObject> mapGoQueue;

    private Map currentMap;

    public Map LoadRecruitmentMap()
    {
        currentMap = recruitmentMap;
        currentMap.Load();
        return currentMap;
    }
    
    public Map LoadNextMap()
    {
        currentMap.Unload();

        currentMap = mapGoQueue.Dequeue().GetComponent<Map>();
        currentMap.Load();
        floor++;
        
        return currentMap;
    }

    public bool OnLastMap()
    {
        return mapGoQueue.Count == 0;
    }

    private void TypeCountsToPercentages(int total)
    {
        int farthestUpperKey = 0;
        float farthestUpperValue = 1f;
        int farthestLowerKey = 0;
        float farthestLowerValue = -1f;
        int totalPercentage = 0;
        
        foreach (int key in typePercentages.Select(pair => pair.Key).ToList())
        {
            float v = (float) ((double) typePercentages[key] / total * 20);
            float distTo05 = v % 1 - 0.5f;
            
            if (distTo05 < 0 && distTo05 > farthestLowerValue)
            {
                farthestLowerKey = key;
                farthestLowerValue = distTo05;
            } 
            else if (distTo05 >= 0 && distTo05 < farthestUpperValue)
            {
                farthestUpperKey = key;
                farthestUpperValue = distTo05;
            }
            
            typePercentages[key] = Mathf.RoundToInt(v) * 5;
            totalPercentage += (int) typePercentages[key];
        }

        if (totalPercentage > 100)
        {
            typePercentages[farthestUpperKey] += 100 - totalPercentage;
        }
        else if (totalPercentage < 100)
        {
            typePercentages[farthestLowerKey] += 100 - totalPercentage;
        }
    }

    private void OnEnable()
    {
        var rd = new Random();
        mapGoQueue = new Queue<GameObject>(mapGOList.OrderBy(go => rd.Next())
            .Take(mapGOList.Length < mapsPerGame ? mapGOList.Length : mapsPerGame));

        int total = 0;
        
        foreach (FieldInfo field in typeof(SpecialistType).GetFields())
        {
            typePercentages.Add((int) field.GetValue(null), 0f);
        }

        foreach (BehaviourNode node in mapGoQueue.SelectMany(mapGO =>
            mapGO.GetComponent<Map>().nodesParent.GetComponentsInChildren<BehaviourNode>()))
        {
            /*switch (node.type)
            {
                case GameTypes.NodeDomainType.None:
                    continue;
                case GameTypes.NodeDomainType.Random:
                    node.SetRandomType();
                    break;
            }*/

            typePercentages[2]++;
            total++;

        }

        TypeCountsToPercentages(total);
    }
}
