using System;
using System.Collections;
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
    [SerializeField] private TextMeshProUGUI[] texts;
    [SerializeField] private ButtonsReferences[] buttons;
    

    public IEnumerator KeyWordHandler(string sceneName)
    {
        foreach (MiniGameScriptableObject t in microGameList.MiniGames)
        {
            if (t.MiniGameSceneName != sceneName) continue;
                
            PlayKeyword( t.MiniGameInput[0], t.MiniGameKeyword);
            break;
        }

        while (director.state == PlayState.Playing) yield return null;
    }

    private void PlayKeyword(MiniGameScriptableObject.ButtonsNames buttonName, string keyword)
    {
        Sprite buttonSprite = null;
        
        foreach (TextMeshProUGUI text in texts)
        {
            text.text = keyword;
        }
        
        foreach (ButtonsReferences buttonRef in buttons)
        {
            if (buttonRef.inputRef == buttonName)
            {
                buttonSprite = buttonRef.spriteAsset;
            }
        }

        if (buttonSprite == null)
        {
            Debug.Log("Pas de référence de sprite d'input pour " + buttonName.ToString());
        }
        else inputPlaceholder.sprite = buttonSprite;
        
        director.Play();    
    }

}

[Serializable]
public class ButtonsReferences
{
    [FormerlySerializedAs("InputRef")] public MiniGameScriptableObject.ButtonsNames inputRef;
    [FormerlySerializedAs("SpriteAsset")] public Sprite spriteAsset;
}
