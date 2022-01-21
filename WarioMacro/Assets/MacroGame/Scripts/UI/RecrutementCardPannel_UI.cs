using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RecrutementCardPannel_UI : MonoBehaviour
{

    [SerializeField] RectTransform allPannels;

    [Header("Card One")]
    [SerializeField] Image cardOneBorder;
    [SerializeField] Image cardOnePortrait;

    [Header("Card Two")]
    [SerializeField] Image cardTwoBorder;
    [SerializeField] Image cardTwoPortrait;

    bool recruitmentActive = false;
    private int counter = 0;
    private void Update()
    {

    }
    public void ActivateRecruitement()
    {
        recruitmentActive = true;
    }

    public void ToggleWindow(bool toggle)
    {
        allPannels.gameObject.SetActive(toggle);
    }

    public void SetCards(Character cardOne, Character cardTwo)
    {
        cardOnePortrait.sprite = cardOne.cardSprite;
        cardTwoPortrait.sprite = cardTwo.cardSprite;

    }
}
