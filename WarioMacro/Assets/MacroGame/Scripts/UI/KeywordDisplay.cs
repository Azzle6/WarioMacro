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
    public TMP_Text BlueTextplaceholder;
    public TMP_Text PinkTextplaceholder;
    public TMP_Text WhiteTextplaceholder;

    private void Start()
    {
        StartCoroutine("testPlay");
    }

    IEnumerator testPlay()
    {
        yield return new WaitForSeconds(4);
        PlayKeyword(ButtonsReferences.ButtonsNames.A, "Pouet");
        yield return new WaitForSeconds(4);
        PlayKeyword(ButtonsReferences.ButtonsNames.B, "OUI BAGUETTE");
    }

    public void PlayKeyword(ButtonsReferences.ButtonsNames buttonName, string Keyword)
    {
        Sprite buttonSprite = null;
        BlueTextplaceholder.text = Keyword;
        WhiteTextplaceholder.text = Keyword;
        PinkTextplaceholder.text = Keyword;
        
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
        
        director.Play();


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
