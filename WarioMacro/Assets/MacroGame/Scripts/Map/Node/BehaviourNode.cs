using System.Collections.Generic;
using GameTypes;
using UnityEngine;

public class BehaviourNode : Node
{
    [HideInInspector] public int microGamesNumber;
    public NodeBehaviour behaviour;
    
    [SerializeField] private SpriteRenderer logoSpriteRenderer;

    private static float primaryDomainPercentage => GameConfig.instance.nodePrimaryDomainPercentage;
    private static float doubleDomainPercentage => GameConfig.instance.nodeDoubleDomainPercentage;
    private static float mgSingleDomainPercentage => GameConfig.instance.mgSingleDomainPercentage;
    private static float mgPrimaryDomainPercentage => GameConfig.instance.mgPrimaryDomainPercentage;
    private static float mgSecondaryDomainPercentage => GameConfig.instance.mgSecondaryDomainPercentage;

    private int[] mgDomains;
    private int primaryDomain = NodeDomainType.None;
    private int secondaryDomain = NodeDomainType.None;

    public void SetRandomDomain(int primaryDomain, int[] secondaryDomains)
    {
        if (behaviour == NodeBehaviour.White) return;

        if (Random.Range(0f, 100f) < primaryDomainPercentage)
        {
            this.primaryDomain = primaryDomain;
            //sRenderer.sprite = Resources.Load<SpriteListSO>("NodeSprites").nodeSprites[primaryDomain - 1];
            logoSpriteRenderer.sprite = Resources.Load<SpriteListSO>("NodeLogoSprites").nodeSprites[primaryDomain - 2];

            if (Random.Range(0f, 100f) < doubleDomainPercentage)
            {
                secondaryDomain = secondaryDomains[Random.Range(0, secondaryDomains.Length)];
            }
        }
        else
        {
            secondaryDomain = secondaryDomains[Random.Range(0, secondaryDomains.Length)];
            //sRenderer.sprite = Resources.Load<SpriteListSO>("NodeSprites").nodeSprites[secondaryDomain - 1];
            logoSpriteRenderer.sprite = Resources.Load<SpriteListSO>("NodeLogoSprites").nodeSprites[secondaryDomain - 2];
        }
        
        Debug.Log($"{this.primaryDomain} : {secondaryDomain}");
    }

    public int[] GetMGDomains()
    {
        if (mgDomains != null) return mgDomains;

        if (primaryDomain != NodeDomainType.None)
        {
            return secondaryDomain != NodeDomainType.None ? MGDomainsDoubleDomain() : MGDomainsSingleDomain(primaryDomain);
        }

        if (secondaryDomain != NodeDomainType.None)
        {
            return MGDomainsSingleDomain(secondaryDomain);
        }
        
        mgDomains = new int[microGamesNumber];
        Debug.Log(mgDomains[0]);
        return mgDomains;
    }

    private int[] MGDomainsSingleDomain(int concernedDomain)
    {
        mgDomains = new int[microGamesNumber];
        bool domainGiven = false;

        for (int i = 0; i < microGamesNumber; i++)
        {
            if (Random.Range(0f, 100f) < mgSingleDomainPercentage)
            {
                mgDomains[i] = concernedDomain;
                domainGiven = true;
                continue;
            }
            mgDomains[i] = NodeDomainType.None;
        }

        if (!domainGiven)
        {
            mgDomains[Random.Range(0, microGamesNumber)] = concernedDomain;
        }

        return mgDomains;
    }

    private int[] MGDomainsDoubleDomain()
    {
        mgDomains = new int[microGamesNumber];
        var noDomainMGs = new List<int>();
        bool pDomainGiven = false;
        bool sDomainGiven = false;

        for (int i = 0; i < microGamesNumber; i++)
        {
            if (Random.Range(0f, 100f) < mgPrimaryDomainPercentage)
            {
                mgDomains[i] = primaryDomain;
                pDomainGiven = true;
                continue;
            }

            if (Random.Range(0f, 100f) < mgSecondaryDomainPercentage)
            {
                mgDomains[i] = secondaryDomain;
                sDomainGiven = true;
                continue;
            }
            
            mgDomains[i] = NodeDomainType.None;
            noDomainMGs.Add(i);
        }
        

        if (!pDomainGiven)
        {
            if (noDomainMGs.Count == 0)
            {
                mgDomains[Random.Range(0, microGamesNumber)] = primaryDomain;
                return mgDomains;
            }
            mgDomains[noDomainMGs[Random.Range(0, noDomainMGs.Count)]] = primaryDomain;
        }

        if (sDomainGiven) return mgDomains;
        
        if (noDomainMGs.Count == 0)
        {
            mgDomains[Random.Range(0, microGamesNumber)] = secondaryDomain;
            return mgDomains;
        }
        mgDomains[noDomainMGs[Random.Range(0, noDomainMGs.Count)]] = secondaryDomain;

        return mgDomains;
    }
    
    
}
