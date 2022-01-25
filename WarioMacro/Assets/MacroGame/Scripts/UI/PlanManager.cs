using GameTypes;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlanManager : MonoBehaviour
{
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private MapManager MapMana;
    [SerializeField] private GameObject PlanObject;
    [SerializeField] private Transform[] DomainsPlaces;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private Button startGameButton;
    [SerializeField] private SpriteListSO domainsVisu;
    [SerializeField] private ScoreMultiplier[] multiplierList;
    
    private ScoreMultiplier currentSelectedMultiplier;
    private bool isOpen;

    private void Start()
    {
        if(multiplierList != null) currentSelectedMultiplier = multiplierList[0];
        scoreManager.scoreMultiplier = currentSelectedMultiplier.multiplierValue;
        UpdateGOButton();
    }

    private void Update()
    {
        if(InputManager.GetKeyDown(ControllerKey.B, true) && isOpen) ClosePlan();
        
    }

    public void OpenPlan()
    {
        PlanObject.SetActive(true);
        isOpen = true;
        InputManager.lockInput = true;
        UpdateDomains();
    }

    public void ClosePlan()
    {
        PlanObject.SetActive(false);
        InputManager.lockInput = false;
        GameController.OnInteractionEnd();
        DisableDomains();
    }

    private void UpdateDomains()
    {
        for (int i = 0; i < MapMana.phaseDomainsArray.Length - 1; i++)
        {
            var normPhase = (NormalPhaseDomains) MapMana.phaseDomainsArray[i];
            
            SetDomain(i, 0, domainsVisu.nodeSprites[normPhase.primaryDomain - SpecialistType.Brute]);
            
            for (int j = 0; j < normPhase.secondaryDomains.Length; j++)
            {
                SetDomain(i, 1 + j, domainsVisu.nodeSprites[normPhase.secondaryDomains[j] - SpecialistType.Brute]);
            }
        }
        
        
        LastPhaseDomains lastPhase = (LastPhaseDomains) MapMana.phaseDomainsArray[MapMana.phaseDomainsArray.Length - 1];

        for (var i = 0; i < lastPhase.primaryDomains.Length; i++)
        {
            SetDomain(2, i, domainsVisu.nodeSprites[lastPhase.primaryDomains[i] - SpecialistType.Brute]);
        }

        if (lastPhase.secondaryDomain != 0)
        {
            SetDomain(2, 2, domainsVisu.nodeSprites[lastPhase.secondaryDomain - SpecialistType.Brute]);
        }
    }

    private void DisableDomains()
    {
        foreach (Transform domainsPlace in DomainsPlaces)
        {
            for (int i = 0; i < domainsPlace.childCount; i++)
            {
                domainsPlace.GetChild(i).gameObject.SetActive(false);
            }
        }
    }


    public void SelectMultiplier(int selectedMultIndex )
    {
        currentSelectedMultiplier = multiplierList[selectedMultIndex];
        scoreManager.scoreMultiplier = currentSelectedMultiplier.multiplierValue;
        UpdateGOButton();
    }

    private void SetDomain(int domainIndex, int childIndex, Sprite sprite)
    {
        DomainsPlaces[domainIndex].GetChild(childIndex).GetChild(0).GetComponent<Image>().sprite = sprite;
        DomainsPlaces[domainIndex].GetChild(childIndex).gameObject.SetActive(true);
    }

    private void UpdateGOButton()
    {
        priceText.text = currentSelectedMultiplier.boostPrice + "$";
        if (scoreManager.currentMoney < currentSelectedMultiplier.boostPrice)
        {
             // "Price : " + currentSelectedMultiplier.boostPrice + "\n Too expensive !"; // TODO : change start button text
            startGameButton.interactable = false;
        }
        else
        {
            // "Price : " + currentSelectedMultiplier.boostPrice + "\n You can go !";
            startGameButton.interactable = true;
        }
    }

}

[System.Serializable]
public class ScoreMultiplier
{
    public string name;
    [FormerlySerializedAs("multiplicatorValue")] public int multiplierValue;
    public float boostPrice;

}
