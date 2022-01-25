using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanManager : MonoBehaviour
{
    public float CurrentSelectedMultiplicator;
    private NormalSecurityDomain[] secondaryDomains;
    private MainSecurityDomain MainDomain;

    public void SelectMultiplicator(float selectedMult )
    {
        CurrentSelectedMultiplicator = selectedMult;
    }
    
    

}

[System.Serializable]
public class NormalSecurityDomain
{
    public Image[] SecondaryDomainsPlaces;
    public Image MainDomainsPlaces;
}

[System.Serializable]
public class MainSecurityDomain
{
    public Image[] MainDomainsPlaces;
}
