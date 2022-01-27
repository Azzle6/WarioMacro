using System;
using GameTypes;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

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
    [SerializeField] private Animator startGameButtonAnimator;
    [SerializeField] private SpriteListSO domainsVisu;
    [SerializeField] private ScoreMultiplier[] multiplierList;
    [SerializeField] private Animator[] multiplierAnimators;

    private ScoreMultiplier currentSelectedMultiplier;
    private bool isOpen;
    private static readonly int isSelected = Animator.StringToHash("isSelected");
    private static readonly int @select = Animator.StringToHash("Select");

    public void OpenPlan()
    {
        PlanObject.SetActive(true);
        AudioManager.MacroPlaySound("MapEnter");
        isOpen = true;
        InputManager.lockInput = true;
        currentSelectedMultiplier = multiplierList[0];
        scoreManager.scoreMultiplier = currentSelectedMultiplier.multiplierValue;
        multiplierAnimators[0].Play("Plan_Multiplier_Selected");
        UpdateGOButton();
        UpdateDomains();
        
    }

    public void ClosePlan(bool playSound)
    {
        PlanObject.SetActive(false);
        isOpen = false;
        if (playSound)
            AudioManager.MacroPlaySound("MapExit");
        
        InputManager.lockInput = false;
        GameController.OnInteractionEnd();
        DisableDomains();
    }

    public void SelectMultiplier(int selectedMultIndex )
    {
        currentSelectedMultiplier = multiplierList[selectedMultIndex];
        scoreManager.scoreMultiplier = currentSelectedMultiplier.multiplierValue;
        UpdateGOButton();

        multiplierAnimators[selectedMultIndex].SetBool(isSelected, true);
        multiplierAnimators[selectedMultIndex].SetTrigger(@select);
        multiplierAnimators[(selectedMultIndex + 1) % 3].SetBool(isSelected, false);
        multiplierAnimators[(selectedMultIndex + 2) % 3].SetBool(isSelected, false);
    }

    private void GenerateFloorCounts()
    {
        floorCountTexts[0].text = GameControllerSO.instance.firstPhaseMinFloorCount + " <> " + GameControllerSO.instance.firstPhaseMaxFloorCount;
        floorCountTexts[1].text = GameControllerSO.instance.secondPhaseMinFloorCount + " <> " + GameControllerSO.instance.secondPhaseMaxFloorCount;
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
        if (CharacterManager.instance.playerTeam.Count < 4)
        {
            startGameText.text = (4 - CharacterManager.instance.playerTeam.Count) + " more members to start...";
            EventSystem.current.SetSelectedGameObject(multiplierAnimators[0].gameObject);
            startGameButton.interactable = false;
        }
        else if (scoreManager.currentMoney < currentSelectedMultiplier.boostPrice)
        {
            startGameText.text = "Too expensive !";
            EventSystem.current.SetSelectedGameObject(multiplierAnimators[0].gameObject);
            startGameButton.interactable = false;
        }
        else
        {
            startGameText.text =  "Go !";
            startGameButton.interactable = true;
            EventSystem.current.SetSelectedGameObject(startGameButton.gameObject);
            startGameButtonAnimator.SetBool("hovered", true);
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
        if(InputManager.GetKeyDown(ControllerKey.B, true) && isOpen) ClosePlan(true);
    }
}

[System.Serializable]
public class ScoreMultiplier
{
    public string name;
    [FormerlySerializedAs("multiplicatorValue")] public int multiplierValue;
    public float boostPrice;

}
