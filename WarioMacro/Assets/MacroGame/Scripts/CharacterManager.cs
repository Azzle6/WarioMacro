using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameTypes;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

// ReSharper disable once CheckNamespace
public class CharacterManager : MonoBehaviour
{
    public CharacterList[] allAvailableCharacters;
    public int totalCharacterCount = 4;
    [HideInInspector] public bool isTeamFull;

    [SerializeField] private GameObject chooseCharacterGO;
    [SerializeField] private Transform buttonsParent;
    
    public readonly Stack<Character> playerTeam = new Stack<Character>();
    private GameObject[] buttonGOList;
    private int currentCount;

    public int SpecialistOfTypeInTeam(int type)
    {
        return playerTeam.Count(c => c.characterType == type);
    }
    
    public IEnumerator DisplayRecruitmentChoice(int charaType)
    {
        CharacterList choices = allAvailableCharacters.First(list => list.type == charaType);
        int choicesCount = buttonGOList.Length < choices.count ? buttonGOList.Length : choices.count;

        if (choices.count == 0)
        {
            Debug.LogError("Plus de persos "+ charaType +" disponibles.");
            yield break;
        }
        
        ResetUI();
        
        //Re-positionnement automatique
        chooseCharacterGO.transform.localPosition = new Vector3(-200 * (choicesCount - 1), 0, 0);

        for (int i = 0; i < choicesCount; i++)
        {
            ShowCharacterCard(choices, i, i);
        }

        yield return WaitForTeamChange();
    }

    public IEnumerator AddDifferentSpecialist(int type)
    {
        var choices = allAvailableCharacters
            .Where(cList => cList.type != CharacterType.Scoundrel && cList.type != type && !cList.IsEmpty()).ToList();

        CharacterList charaList = choices[Random.Range(0, choices.Count)];
        
        ResetUI();
        
        //Re-positionnement automatique
        chooseCharacterGO.transform.localPosition = new Vector3(0, 0, 0);

        ShowCharacterCard(charaList, Random.Range(0, charaList.count), 0);

        yield return WaitForTeamChange();
    }

    public IEnumerator AddDefaultCharacter()
    {
        CharacterList choices = allAvailableCharacters.First(list => list.type == CharacterType.Scoundrel);
        int charaLeft = choices.count;

        if (charaLeft == 0)
        {
            Debug.LogWarning("Plus de persos par défaut disponibles.");
            yield break;
        }
        int randomN = Random.Range(0, charaLeft);
        
        ResetUI();
        
        //Re-positionnement automatique
        chooseCharacterGO.transform.localPosition = new Vector3(0, 0, 0);

        ShowCharacterCard(choices, randomN, 0);

        yield return WaitForTeamChange();
    }

    private void ResetUI()
    {
        chooseCharacterGO.SetActive(true);

        foreach (GameObject go in buttonGOList)
        {
            go.SetActive(false);
        }
    }
    
    private void ShowCharacterCard(CharacterList list, int charaIndex, int buttonIndex)
    {
        // Reset Button
        buttonGOList[buttonIndex].SetActive(true);
        buttonGOList[buttonIndex].GetComponent<Image>().sprite = list.Get(charaIndex).cardSprite;
        buttonGOList[buttonIndex].GetComponent<Button>().onClick.RemoveAllListeners();
            
        //Ajoute un listener au bouton pour qu'il ajoute le bon personnage
        buttonGOList[buttonIndex].GetComponent<Button>().onClick.AddListener(() => chooseCharacterGO.SetActive(false));
        buttonGOList[buttonIndex].GetComponent<Button>().onClick.AddListener(delegate { AddCharacter(list, charaIndex); });
    }

    private IEnumerator WaitForTeamChange()
    {
        int currentTeamCount = playerTeam.Count;

        while (currentTeamCount == playerTeam.Count) yield return null;
    }

    private void AddCharacter(CharacterList characterList, int index)
    {
        if (isTeamFull)
        {
            Debug.LogError("Character added when the team is complete.");
            return;
        }
        
        playerTeam.Push(characterList.Get(index));
        characterList.RemoveAt(index);
        currentCount++;

        if (currentCount == totalCharacterCount)
        {
            isTeamFull = true;
        }
        Debug.Log("personnage " + playerTeam.Peek() + " a été ajouté à l'équipe!");
    }
    
    public void LoseCharacter()
    {
        playerTeam.Pop();
    }

    private void Start()
    {
        buttonGOList = new GameObject[buttonsParent.childCount];
        for (int i = 0; i < buttonGOList.Length; i++)
        {
            buttonGOList[i] = buttonsParent.GetChild(i).gameObject;
        }
    }
}
