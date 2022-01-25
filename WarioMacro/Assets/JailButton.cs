using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JailButton : MonoBehaviour
{
    public TextMeshProUGUI priceTMP;
    public TextMeshProUGUI timeLeftTMP;
    public Image portrait;
    private CharacterManager.Imprisoned character;
    
    public void SetJail(CharacterManager.Imprisoned imprisoned)
    {
        priceTMP.text = imprisoned.price.ToString() +"$";
        timeLeftTMP.text = imprisoned.turnLeft.ToString();
        portrait.sprite = imprisoned.character.cardSprite;
        character = imprisoned;
    }

    public void ReleaseCharacter()
    {
        CharacterManager.instance.FreeImprisoned(character);
        CharacterManager.instance.SetJailUI();
    }
}
