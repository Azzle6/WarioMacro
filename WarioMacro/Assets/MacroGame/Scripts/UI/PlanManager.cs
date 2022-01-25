using System;
using System.Collections;
using System.Collections.Generic;
using GameTypes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlanManager : MonoBehaviour
{
    [SerializeField] private ScoreManager ScoreMana;
    [SerializeField] private MapManager MapMana;
    [SerializeField] private GameObject PlanObject;
    
    private NormalSecurityDomain[] secondaryDomainsArray;
    private MainSecurityDomain MainDomain;

    [SerializeField] private GameObject[] DomainsPlaces;
    [SerializeField] private GameObject GoButtonPriceObject;
    [SerializeField] private GameObject GoButton;
    [SerializeField] private GameObject SecondaryDomainTemplate;
    [SerializeField] private GameObject PrimaryDomainTemplate;
    [SerializeField] private int currentMoney;
    [SerializeField] private SpriteListSO domainsVisu;

    [SerializeField]private ScoreMultiplicator[] MultiplicatorsList;
    [SerializeField]private ScoreMultiplicator CurrentSelectedMultiplicator;
    [SerializeField] private bool canPay;
    private bool isOpen;

    private void Start()
    {
        if(MultiplicatorsList != null) CurrentSelectedMultiplicator = MultiplicatorsList[0];
    }

    private void Update()
    {
        if(InputManager.GetKeyDown(ControllerKey.B) && isOpen) ClosePlan();
        
    }

    public void OpenPlan()
    {
        PlanObject.SetActive(true);
        UpdateDomains();
    }

    public void ClosePlan()
    {
        PlanObject.SetActive(false);
        GameController.OnInteractionEnd();
    }

    private void UpdateDomains()
    {
        for (int i = 0; i < MapMana.phaseDomainsArray.Length - 1; i++)
        {
            NormalPhaseDomains normPhase = (NormalPhaseDomains) MapMana.phaseDomainsArray[i];
            
            //Debug.Log("i = " + i);
            for (int j = 0; j < normPhase.secondaryDomains.Length; j++)
            {
                //Debug.Log("j = " + j);
                GameObject newSecondaryDomainVisual = Instantiate(SecondaryDomainTemplate, DomainsPlaces[i].transform);
                
                newSecondaryDomainVisual.GetComponent<Image>().sprite = domainsVisu.nodeSprites[normPhase.secondaryDomains[j] - SpecialistType.Brute];
            }
            
            GameObject newPrimaryDomainVisual = Instantiate(PrimaryDomainTemplate, DomainsPlaces[i].transform);
                
            newPrimaryDomainVisual.GetComponent<Image>().sprite = domainsVisu.nodeSprites[normPhase.primaryDomain - SpecialistType.Brute];
        }
        
        
        
        LastPhaseDomains lastPhase = (LastPhaseDomains) MapMana.phaseDomainsArray[MapMana.phaseDomainsArray.Length - 1];

        foreach (int primDomains in lastPhase.primaryDomains)
        {
            GameObject primaryDomainVisual = Instantiate(PrimaryDomainTemplate, DomainsPlaces[DomainsPlaces.Length - 1].transform);
                
            primaryDomainVisual.GetComponent<Image>().sprite = domainsVisu.nodeSprites[primDomains - SpecialistType.Brute];
        }
        

        
    }


    public void SelectMultiplicator(int selectedMultIndex )
    {
        CurrentSelectedMultiplicator = MultiplicatorsList[selectedMultIndex];
        UpdateGOButton();
    }

    private void UpdateGOButton()
    {
        
        if (currentMoney < CurrentSelectedMultiplicator.boostPrice)
        {
            GoButtonPriceObject.GetComponentInChildren<TMP_Text>().text = "Price : " + CurrentSelectedMultiplicator.boostPrice + "\n Too expensive !";
            canPay = false;
        }
        else
        {
            GoButtonPriceObject.GetComponentInChildren<TMP_Text>().text = "Price : " + CurrentSelectedMultiplicator.boostPrice + "\n You can go !";
            canPay = true;
        }
    }

}

[System.Serializable]
public class ScoreMultiplicator
{
    public string name;
    public float multiplicatorValue;
    public float boostPrice;

}

[System.Serializable]
public class NormalSecurityDomain
{
    public Image[] SecondaryDomainsPlaces;
    public Image MainDomainsPlace;
}

[System.Serializable]
public class MainSecurityDomain
{
    public Image[] MainDomainsPlaces;
}
