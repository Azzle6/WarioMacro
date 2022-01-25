using System.Collections.Generic;
using System.Linq;
using GameTypes;
using UnityEngine;
using Random = UnityEngine.Random;

// ReSharper disable once CheckNamespace
public class MapManager : MonoBehaviour
{
    public static int currentPhase { get; private set; }
    public static int floor { get; private set; }
    public GameObject currentMapGO { get; private set; }

    [SerializeField] private GameSettingsManager settingsManager;
    [SerializeField] private GameConfig config;
    [SerializeField] private Transform mapParent;
    [SerializeField] private Map recruitmentMap;
    [SerializeField] private Map astralPathMap;
    [SerializeField] private GameObject[] mapPrefabList;
    private Queue<GameObject> mapPrefabQueue;
    public IPhaseDomains[] phaseDomainsArray;
    private Map currentMap;
    [HideInInspector] public int[] phaseFloorThresholds = new int[2];
    

    public Map LoadRecruitmentMap()
    {
        GeneratePhaseFloorCount();
        GeneratePhasesDomains();
        currentMap = recruitmentMap;
        currentMap.Load();
        return currentMap;
    }
    
    public Map LoadAstralPath()
    {
        currentMap.Unload();
        currentMap = astralPathMap;
        currentMap.Load();
        return currentMap;
    }
    
    public Map LoadNextMap()
    {
        currentMap.Unload();
        Destroy(currentMapGO);

        if (mapPrefabQueue.Count < 6)
        {
            RefillMapQueue();
        }

        currentMapGO = Instantiate(mapPrefabQueue.Dequeue(), mapParent);
        currentMap = currentMapGO.GetComponent<Map>();
        currentMap.Load();
        floor++;

        if (currentPhase < 2 && floor > phaseFloorThresholds[currentPhase])
        {
            UpdatePhase();
        }
        
        GenerateMapNodesDomains(currentMap);
        
        return currentMap;
    }

    public void GeneratePhaseFloorCount()
    {
        phaseFloorThresholds[0] = Random.Range(config.firstPhaseMinFloorCount, config.firstPhaseMaxFloorCount + 1);
        phaseFloorThresholds[1] = Random.Range(config.secondPhaseMinFloorCount, config.secondPhaseMaxFloorCount + 1);
    }

    private void GeneratePhasesDomains()
    {
        var notUsedDomains = new List<int>(SpecialistType.GetTypes());
        phaseDomainsArray = new IPhaseDomains[3];
        for (int i = 0; i < phaseDomainsArray.Length - 1; i++)
        {
            GenerateNormalPhaseDomains(i, ref notUsedDomains);
        }
        GenerateLastPhaseDomains(notUsedDomains);
        
    }

    public void SkipToNextMap()
    {
        currentMap.SelectLastNode();
    }

    private void GenerateNormalPhaseDomains(int phase, ref List<int> notUsedDomains)
    {
        var secondaryDomains = new List<int>();
        var domains = new List<int>(SpecialistType.GetTypes());
        
        int primaryDomain = GetDomainFromList(ref domains, ref notUsedDomains);
        
        secondaryDomains.Add(GetDomainFromList(ref domains, ref notUsedDomains));

        if (Random.Range(0f, 100f) < config.phaseDoubleDomainPercentage)
        {
            secondaryDomains.Add(GetDomainFromList(ref domains, ref notUsedDomains));
        }

        phaseDomainsArray[phase] = new NormalPhaseDomains(primaryDomain, secondaryDomains.ToArray());
    }

    private void GenerateLastPhaseDomains(List<int> notUsedDomains)
    {
        var primaryDomains = new int[2];
        int secondaryDomain = 0;
        var domains = new List<int>(SpecialistType.GetTypes());

        switch (notUsedDomains.Count)
        {
            case 4:
                // modify an already set domain
                ReplaceDomain(ref notUsedDomains, ref domains, Random.Range(0, 4));
                goto case 3;
            case 3:
                // 1st in not used list
                primaryDomains[0] = GetDomainFromList(ref notUsedDomains, ref domains);
                
                // 2d in not used list
                primaryDomains[1] = GetDomainFromList(ref notUsedDomains, ref domains);
                break;
            case 2:
                // 1st in not used list
                primaryDomains[0] = GetDomainFromList(ref notUsedDomains, ref domains);
                // 2d random
                primaryDomains[1] = GetDomainFromList(ref domains);

                break;
            default:
                // 1st random
                primaryDomains[0] = GetDomainFromList(ref domains);
                
                // 2d random
                primaryDomains[1] = GetDomainFromList(ref domains);
                
                break;
        }

        // 3rd is completely random
        if (Random.Range(0f, 100f) < config.lastPhaseSecondaryDomainPercentage)
        {
            secondaryDomain = GetDomainFromList(ref domains);
        }

        phaseDomainsArray[phaseDomainsArray.Length - 1] = new LastPhaseDomains(primaryDomains, secondaryDomain);
    }

    private static int GetDomainFromList(ref List<int> toUse)
    {
        List<int> keepUpdated = null;
        return GetDomainFromList(ref toUse, ref keepUpdated);
    }

    private static int GetDomainFromList(ref List<int> toUse, ref List<int> keepUpdated)
    {
        int rd = Random.Range(0, toUse.Count);
        int res = toUse[rd];
        keepUpdated?.Remove(toUse[rd]);
        toUse.RemoveAt(rd);
        
        return res;
    }

    private void ReplaceDomain(ref List<int> notUsedDomains, ref List<int> domains, int toReplace)
    {
        int newDomain = GetDomainFromList(ref notUsedDomains, ref domains);
        if (toReplace / 2 == 0)
        {
            ((NormalPhaseDomains) phaseDomainsArray[toReplace % 2]).SetPrimaryDomain(newDomain);
        }
        else
        {
            ((NormalPhaseDomains) phaseDomainsArray[toReplace % 2]).SetSecondaryDomain(newDomain, 0);
        }
    }

    private void GenerateMapNodesDomains(Map map)
    {
        foreach (BehaviourNode node in map.nodesParent.GetComponentsInChildren<BehaviourNode>())
        {
            node.SetRandomDomain(phaseDomainsArray[currentPhase]);
        }
    }

    private void UpdatePhase()
    {
        PlayerPrefs.Save();
        if (currentPhase == 2)
        {
            Debug.LogError("Already at last phase (3)");
        }
        currentPhase++;
        settingsManager.IncreaseDifficulty();
    }

    private void RefillMapQueue()
    {
        PlayerPrefs.Save();
        var rd = new System.Random();
        var currentList = new List<GameObject>(mapPrefabQueue);

        foreach (GameObject go in mapPrefabList.OrderBy(go => rd.Next()))
        {
            if (!AlreadyIn(ref currentList, go))
            {
                mapPrefabQueue.Enqueue(go);
            }
        }
    }

    private static bool AlreadyIn(ref List<GameObject> goList, Object go)
    {
        for (var i = 0; i < goList.Count; i++)
        {
            if (go != goList[i]) continue;
                
            goList.RemoveAt(i);
            return true;
        }

        return false;
    }

    private void OnEnable()
    {
        var rd = new System.Random();
        mapPrefabQueue = new Queue<GameObject>(mapPrefabList.OrderBy(go => rd.Next()));
    }
}
