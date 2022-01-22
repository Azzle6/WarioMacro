using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Serialization;
using UnityEngine.UI;

// ReSharper disable once CheckNamespace
public class KeywordDisplay : MonoBehaviour
{
    [SerializeField] private ScenesReferencesSO microGameList;
    [SerializeField] private PlayableDirector director;
    [SerializeField] private Image inputPlaceholder;
    [SerializeField] private Image secondaryInputPlaceholder;
    [SerializeField] private GameObject secondaryInputGO;
    [SerializeField] private TextMeshProUGUI[] texts;
    [SerializeField] private ButtonsReferences[] buttons;
    

    public IEnumerator KeyWordHandler(string sceneName)
    {
        foreach (MiniGameScriptableObject t in microGameList.MiniGames)
        {
            if (t.MiniGameSceneName != sceneName) continue;
                
            PlayKeyword(t.MiniGameInput, t.MiniGameKeyword);
            break;
        }

        while (director.state == PlayState.Playing) yield return null;
    }

    private void PlayKeyword(IReadOnlyList<MiniGameScriptableObject.ButtonName> buttonNames, string keyword)
    {
        foreach (TextMeshProUGUI text in texts)
        {
            text.text = keyword;
        }

        switch (buttonNames.Count)
        {
            case 0:
                Debug.LogError("Input list is empty");
                break;
            case 1:
                DisplaySingleInput(buttonNames[0]);
                break;
            case 2:
                DisplayMultipleInput(buttonNames);
                break;
            default:
                Debug.LogError("More than 2 inputs in the list");
                break;
        }

        director.Play();    
    }

    private void DisplayMultipleInput(IReadOnlyList<MiniGameScriptableObject.ButtonName> buttonNames)
    {
        secondaryInputGO.SetActive(true);
        bool foundFirstSprite = false;
        bool foundSecondSprite = false;
        
        foreach (ButtonsReferences buttonRef in buttons)
        {
            if (buttonRef.inputRef == buttonNames[0])
            {
                inputPlaceholder.sprite = buttonRef.spriteAsset;
                foundFirstSprite = true;
            }
            else if (buttonRef.inputRef == buttonNames[1])
            {
                secondaryInputPlaceholder.sprite = buttonRef.spriteAsset;
                foundSecondSprite = true;
            }

            if (foundFirstSprite && foundSecondSprite) break;
        }
        
        if (!foundFirstSprite)
        {
            Debug.LogError($"Input name {buttonNames[0]} doesn't exist");
        }
        if (!foundSecondSprite)
        {
            Debug.LogError($"Input name {buttonNames[1]} doesn't exist");
        }
    }

    private void DisplaySingleInput(MiniGameScriptableObject.ButtonName buttonName)
    {
        secondaryInputGO.SetActive(false);
        bool foundSprite = false;
        foreach (ButtonsReferences buttonRef in buttons)
        {
            if (buttonRef.inputRef != buttonName) continue;
            inputPlaceholder.sprite = buttonRef.spriteAsset;
            foundSprite = true;
            break;
        }

        if (!foundSprite)
        {
            Debug.LogError($"Input name {buttonName} doesn't exist");
        }
    }

}

[Serializable]
public class ButtonsReferences
{
    [FormerlySerializedAs("InputRef")] public MiniGameScriptableObject.ButtonName inputRef;
    [FormerlySerializedAs("SpriteAsset")] public Sprite spriteAsset;
}
