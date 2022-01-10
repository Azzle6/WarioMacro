using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class KeywordDisplay : MonoBehaviour
{
    [SerializeField]private PlayableDirector director;
    [SerializeField]private ButtonsReferences[] buttons;
    [SerializeField]private string CurrentKeyword;
    public Image InputPlaceholder;
    public TMP_Text Textplaceholder;
    

    public void PlayKeyword(ButtonsReferences.ButtonsNames buttonName, string Keyword)
    {
        Sprite buttonSprite = null;
        Textplaceholder.text = Keyword;
        foreach (ButtonsReferences ButtonRef in buttons)
        {
            if (ButtonRef.InputRef == buttonName)
            {
                buttonSprite = ButtonRef.SpriteAsset;
            }
        }

        if (buttonSprite == null)
        {
            Debug.Log("Pas de référence de sprite d'input pour " + buttonName.ToString());
        }
        else InputPlaceholder.sprite = buttonSprite;



    }



}

[System.Serializable]
public class ButtonsReferences
{
    public enum ButtonsNames
    {
        X,
        Y,
        A,
        B,
        LeftJoystick,
        RightJoystick,
        LeftTrigger,
        RightTrigger,
        LeftButton,
        RightButton,
        DirectionalButtons
    }

    public ButtonsNames InputRef;
    public Sprite SpriteAsset;
}
