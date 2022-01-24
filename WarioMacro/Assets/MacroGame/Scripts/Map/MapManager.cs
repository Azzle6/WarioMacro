using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GameTypes;
using UnityEngine;
using Random = UnityEngine.Random;

// ReSharper disable once CheckNamespace
public class MapManager : MonoBehaviour
{
    public static int currentPhase { get; private set; } = 0;
    public static int floor { get; private set; } = 0;
    public readonly Dictionary<int, float> typePercentages = new Dictionary<int, float>();

    [SerializeField] private GameSettingsManager settingsManager;
    [SerializeField] private Map recruitmentMap;
    [SerializeField] private Map astralPathMap;
    [SerializeField] private int mapsPerGame = 5;
    [SerializeField] private GameObject[] mapGOList;
    private Queue<GameObject> mapGoQueue;
    private IPhaseDomains[] phaseDomainsArray;
    private int[] phaseFloorThresholds = new int[2] {5, 8}; // TODO : replace with right values
    private Map currentMap;

    public Map LoadRecruitmentMap()
    {
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

        currentMap = mapGoQueue.Dequeue().GetComponent<Map>();
        currentMap.Load();
        floor++;

        if (currentPhase < 2 && floor > phaseFloorThresholds[currentPhase])
        {
            UpdatePhase();
        }
        
        GenerateMapNodesDomains(currentMap);
        
        return currentMap;
    }

    public void GeneratePhasesDomains()
    {
        var notUsedDomains = new List<int>(SpecialistType.GetTypes());
        phaseDomainsArray = new IPhaseDomains[3];
        for (int i = 0; i < phaseDomainsArray.Length - 1; i++)
        {
            GenerateNormalPhaseDomains(i, ref notUsedDomains);
        }
        GenerateLastPhaseDomains(notUsedDomains);
        
    }

    private void GenerateNormalPhaseDomains(int phase, ref List<int> notUsedDomains)
    {
        var domains = new List<int>(SpecialistType.GetTypes());
        
        int rd = Random.Range(0, domains.Count);
        int primaryDomain = domains[rd];
        notUsedDomains.Remove(domains[rd]);
        domains.RemoveAt(rd);
        
        rd = Random.Range(0, domains.Count);
        var secondaryDomains = new List<int> {domains[rd]};
        notUsedDomains.Remove(domains[rd]);
        domains.RemoveAt(rd);

        if (Random.Range(0f, 100f) < GameConfig.instance.phaseDoubleDomainPercentage)
        {
            rd = Random.Range(0, domains.Count);
            secondaryDomains.Add(domains[rd]);
            notUsedDomains.Remove(domains[rd]);
        }

        phaseDomainsArray[phase] = new NormalPhaseDomains(primaryDomain, secondaryDomains.ToArray());
    }

    private void GenerateLastPhaseDomains(IList<int> notUsedDomains)
    {
        var primaryDomains = new int[2];
        var domains = new List<int>(SpecialistType.GetTypes());
        
        int rd;
   
        switch (notUsedDomains.Count)
        {
            case 4:
                // modify an already set domain
                ReplaceDomain(ref notUsedDomains, Random.Range(0, 4));
                goto FirstPrimaryNotUsed;
            case 3:
                goto FirstPrimaryNotUsed;
            case 2:
                // 1st random
                rd = Random.Range(0, domains.Count);
                primaryDomains[0] = domains[rd];
                domains.RemoveAt(rd);

                goto SecondPrimaryNotUsed;
            default:
                // 1st random
                rd = Random.Range(0, domains.Count);
                primaryDomains[0] = domains[rd];
                domains.RemoveAt(rd);
                
                // 2d random
                rd = Random.Range(0, domains.Count);
                primaryDomains[1] = domains[rd];
                domains.RemoveAt(rd);
                
                goto RandomSecondary;
        }
        
        // 1st in not used list
        FirstPrimaryNotUsed :
        rd = Random.Range(0, notUsedDomains.Count);
        primaryDomains[0] = notUsedDomains[rd];
        notUsedDomains.RemoveAt(rd);
        
        // 2d in not used list
        SecondPrimaryNotUsed :
        rd = Random.Range(0, notUsedDomains.Count);
        primaryDomains[1] = notUsedDomains[rd];
        
        // 3rd is completely random
        RandomSecondary :
        rd = Random.Range(0, domains.Count);
        int secondaryDomain = domains[rd];

        phaseDomainsArray[phaseDomainsArray.Length - 1] = new LastPhaseDomains(primaryDomains, secondaryDomain);
    }

    private void ReplaceDomain(ref IList<int> notUsedDomains, int toReplace)
    {
        int rd = Random.Range(0, notUsedDomains.Count);
        if (toReplace / 2 == 0)
        {
            ((NormalPhaseDomains) phaseDomainsArray[toReplace % 2]).SetPrimaryDomain(notUsedDomains[rd]);
        }
        else
        {
            ((NormalPhaseDomains) phaseDomainsArray[toReplace % 2]).SetSecondaryDomain(notUsedDomains[rd], 0);
        }
        notUsedDomains.RemoveAt(rd);
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
        if (currentPhase == 2)
        {
            Debug.LogError("Already at last phase (3)");
        }
        currentPhase++;
        settingsManager.IncreaseDifficulty();
    }

    private void OnEnable()
    {
        var rd = new System.Random();
        mapGoQueue = new Queue<GameObject>(mapGOList.OrderBy(go => rd.Next())
            .Take(mapGOList.Length < mapsPerGame ? mapGOList.Length : mapsPerGame));
        
        // TODO : Obsolete, delete after merging
        foreach (FieldInfo field in typeof(SpecialistType).GetFields())
        {
            typePercentages.Add((int) field.GetValue(null), 0f);
        }
    }
}
