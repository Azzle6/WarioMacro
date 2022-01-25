using System.Collections.Generic;
using GameTypes;
using UnityEngine;
using UnityEngine.UI;

public class BehaviourNode : Node
{
    [HideInInspector] public int microGamesNumber;
    public NodeBehaviour behaviour;
    
    [SerializeField] private Image primaryLogo;
    [SerializeField] private Image secondaryLogo;

    private static float primaryDomainPercentage => GameConfig.instance.nodePrimaryDomainPercentage;
    private static float doubleDomainPercentage => GameConfig.instance.nodeDoubleDomainPercentage;
    private static float mgSingleDomainPercentage => GameConfig.instance.mgSingleDomainPercentage;
    private static float mgPrimaryDomainPercentage => GameConfig.instance.mgPrimaryDomainPercentage;
    private static float mgSecondaryDomainPercentage => GameConfig.instance.mgSecondaryDomainPercentage;

    private int[] mgDomains;
    private int primaryDomain = NodeDomainType.None;
    private int secondaryDomain = NodeDomainType.None;

    public void SetRandomDomain(IPhaseDomains phaseDomains)
    {
        if (behaviour == NodeBehaviour.White) return;

        if (Random.Range(0f, 100f) < primaryDomainPercentage)
        {
            primaryDomain = phaseDomains.GetRandomPrimaryDomain();
            //sRenderer.sprite = Resources.Load<SpriteListSO>("NodeSprites").nodeSprites[primaryDomain - 1];
            primaryLogo.sprite = Resources.Load<SpriteListSO>("NodeLogoSprites").nodeSprites[primaryDomain - 2];

            if (Random.Range(0f, 100f) < doubleDomainPercentage)
            {
                secondaryDomain = phaseDomains.GetRandomSecondaryDomain();
                secondaryLogo.sprite = Resources.Load<SpriteListSO>("NodeLogoSprites").nodeSprites[secondaryDomain - 2];
                secondaryLogo.transform.parent.gameObject.SetActive(true);
            }
        }
        else
        {
            secondaryDomain = phaseDomains.GetRandomSecondaryDomain();
            //sRenderer.sprite = Resources.Load<SpriteListSO>("NodeSprites").nodeSprites[secondaryDomain - 1];
            primaryLogo.sprite = Resources.Load<SpriteListSO>("NodeLogoSprites").nodeSprites[secondaryDomain - 2];
        }
        primaryLogo.transform.parent.gameObject.SetActive(true);
        
    }

    public int GetMGDomain(int index)
    {
        if (index >= microGamesNumber)
        {
            Debug.LogError("Index out of range");
        }
        
        if (mgDomains == null)
            GetMGDomains();

        return mgDomains[index];
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

    public override void DisableNode()
    {
        base.DisableNode();
        if (primaryLogo != null)
        {
            primaryLogo.transform.parent.gameObject.SetActive(false);
        }

        if (secondaryLogo != null)
        {
            secondaryLogo.transform.parent.gameObject.SetActive(false);
        }
        
        
    }
}
