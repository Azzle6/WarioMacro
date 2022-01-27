using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class RecrutementCardPannel_UI : MonoBehaviour
{

    [SerializeField] private RectTransform allPanels;

    [Header("Card One")]
    [SerializeField] private Button cardOneButton;
    [SerializeField] private Image cardOneBorder;
    [SerializeField] private Image cardOnePortrait;

    [Header("Card Two")]
    [SerializeField] private Button cardTwoButton;
    [SerializeField] private Image cardTwoBorder;
    [SerializeField] private Image cardTwoPortrait;

    public void ToggleWindow(bool toggle)
    {
        allPanels.gameObject.SetActive(toggle);
    }

    public void SetCards(Character cardOne, Character cardTwo)
    {
        cardOnePortrait.sprite = cardOne.fullSizeSprite;
        cardTwoPortrait.sprite = cardTwo.fullSizeSprite;

    }
    
    public void ShowCharacterCard(UnityAction call, Character character, int cardIndex)
    {
        Button button;
        Image portrait;
        if (cardIndex == 0)
        {
            button = cardOneButton;
            portrait = cardOnePortrait;
        }
        else
        {
            button = cardTwoButton;
            portrait = cardTwoPortrait;
        }
        
        // Reset Button
        button.gameObject.SetActive(true);
        portrait.sprite = character.fullSizeSprite;
        button.onClick.RemoveAllListeners();
            
        //Ajoute un listener au bouton pour qu'il ajoute le bon personnage
        button.onClick.AddListener(() => gameObject.SetActive(false));
        button.onClick.AddListener(call);
    }
}
