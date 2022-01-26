using System;
using UnityEngine;
using Random = UnityEngine.Random;

// ReSharper disable once CheckNamespace
public interface IPhaseDomains
{
    public int GetRandomPrimaryDomain();
    public int GetRandomSecondaryDomain();
}

public class NormalPhaseDomains : IPhaseDomains
{
    public int primaryDomain;
    public readonly int[] secondaryDomains;

    public NormalPhaseDomains(int primaryDomain, int[] secondaryDomains)
    {
        if (secondaryDomains == null || secondaryDomains.Length < 1)
        {
            throw new ArgumentException();
        }
        
        this.primaryDomain = primaryDomain;
        this.secondaryDomains = secondaryDomains;
    }

    public int GetRandomPrimaryDomain()
    {
        return primaryDomain;
    }

    public int GetRandomSecondaryDomain()
    {
        Debug.Log("last : ca bug " + secondaryDomains[0]);
        if (secondaryDomains.Length > 1)
        {
            Debug.Log("last : ca bug " + secondaryDomains[1]);
        }
        
        return secondaryDomains[Random.Range(0, secondaryDomains.Length)];
    }

    public void SetPrimaryDomain(int domain)
    {
        primaryDomain = domain;
    }

    public void SetSecondaryDomain(int domain, int index)
    {
        secondaryDomains[index] = domain;
    }
    
    public override string ToString()
    {
        return $"{primaryDomain}, {secondaryDomains[0]}" + (secondaryDomains.Length > 1 ? ", " + secondaryDomains[1] : "");
    }
}

public class LastPhaseDomains : IPhaseDomains
{
    public readonly int[] primaryDomains;
    public readonly int secondaryDomain;

    public LastPhaseDomains(int[] primaryDomains, int secondaryDomain)
    {
        if (primaryDomains == null || primaryDomains.Length < 2)
        {
            throw new ArgumentException();
        }
        
        this.primaryDomains = primaryDomains;
        this.secondaryDomain = secondaryDomain;
    }

    public int GetRandomPrimaryDomain()
    {
        return primaryDomains[Random.Range(0, primaryDomains.Length)];
    }

    public int GetRandomSecondaryDomain()
    {
        Debug.Log("last : ca bug " + secondaryDomain);
        return secondaryDomain;
    }

    public override string ToString()
    {
        return $"{primaryDomains[0]}, {primaryDomains[1]}, {secondaryDomain}";
    }
}
