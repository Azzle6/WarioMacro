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
    [SerializeField] private TextMeshProUGUI[] floorCountTexts;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private TextMeshProUGUI startGameText;
    [SerializeField] private Button startGameButton;
    [SerializeField] private SpriteListSO domainsVisu;
    [SerializeField] private ScoreMultiplier[] multiplierList;

    private ScoreMultiplier currentSelectedMultiplier;
    private bool isOpen;

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
    
    public void SelectMultiplier(int selectedMultIndex )
    {
        currentSelectedMultiplier = multiplierList[selectedMultIndex];
        scoreManager.scoreMultiplier = currentSelectedMultiplier.multiplierValue;
        UpdateGOButton();
    }

    private void GenerateFloorCounts()
    {
        for (int i = 0; i < 2; i++)
        {
            int trueFloorCount = MapMana.phaseFloorThresholds[i];
            if (trueFloorCount == GameConfig.instance.firstPhaseMaxFloorCount)
            {
                floorCountTexts[i].text = trueFloorCount.ToString();
                continue;
            }

            int lowerLimit = trueFloorCount - Random.Range(1, 3);
            if (lowerLimit < 1)
                lowerLimit = 1;
            int upperLimit = trueFloorCount + Random.Range(1, 3);
            
            floorCountTexts[i].text = lowerLimit + " <> " + upperLimit;
        }
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
            startGameText.text = "Too expensive !";
            startGameButton.interactable = false;
        }
        else
        {
            
            startGameText.text =  "Go !";
            startGameButton.interactable = true;
        }
        if (CharacterManager.instance.playerTeam.Count < 4)
        {
            startGameText.text = (4 - CharacterManager.instance.playerTeam.Count) + " more members to start...";
        }
    }
    
    private void Start()
    {
        if(multiplierList != null) currentSelectedMultiplier = multiplierList[0];
        scoreManager.scoreMultiplier = currentSelectedMultiplier.multiplierValue;
        UpdateGOButton();
        GenerateFloorCounts();
    }

    private void Update()
    {
        if(InputManager.GetKeyDown(ControllerKey.B, true) && isOpen) ClosePlan();
    }

}

[System.Serializable]
public class ScoreMultiplier
{
    public string name;
    [FormerlySerializedAs("multiplicatorValue")] public int multiplierValue;
    public float boostPrice;

}
