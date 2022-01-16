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
    [SerializeField] private GameObject cardButtonTemplate;
    
    private readonly Stack<Character> playerTeam = new Stack<Character>();
    private int currentCount;

    public int SpecialistOfTypeInTeam(int type)
    {
        return playerTeam.Count(c => c.characterType == type);
    }
    
    public void DisplayRecruitmentChoice(int charaType)
    {
        CharacterList choices = allAvailableCharacters.First(list => list.type == charaType);
        int charaLeft = choices.count;

        chooseCharacterGO.SetActive(true);

        if (charaLeft == 0)
        {
            Debug.LogError("Plus de persos "+ charaType +" disponibles.");
            return;
        }
        
        //Re-positionnement automatique
        float spacing = 900 / Mathf.Pow(3, (charaLeft - 1));
        chooseCharacterGO.transform.localPosition =
            charaLeft > 1 ? new Vector3(-200, 0, 0) : Vector3.zero;
        

        chooseCharacterGO.GetComponent<HorizontalLayoutGroup>().spacing = spacing;

        //Instancier toutes les cartes dont on a besoin
        List<GameObject> displayedGO = choices
            .Select(characterSO => Instantiate(cardButtonTemplate, chooseCharacterGO.transform)).ToList();

        Destroy(chooseCharacterGO.transform.GetChild(0).gameObject);

        for (int i = 0; i < charaLeft; i++)
        {
            displayedGO[i].GetComponent<Image>().sprite = choices.Get(i).cardSprite;

            //Ajoute un listener au bouton pour qu'il ajoute le bon personnage
            int i1 = i;
            displayedGO[i].GetComponent<Button>().onClick
                .AddListener(delegate { AddCharacter(choices, i1); });
        }
    }

    public void AddDifferentSpecialist(int type)
    {
        var choices = allAvailableCharacters
            .Where(cList => cList.type != CharacterType.Scoundrel && cList.type != type && !cList.IsEmpty()).ToList();

        CharacterList charaList = choices[Random.Range(0, choices.Count)];
        AddCharacter(charaList, Random.Range(0, charaList.count));
    }

    public void AddDefaultCharacter()
    {
        CharacterList choices = allAvailableCharacters.First(list => list.type == CharacterType.Scoundrel);
        int charaLeft = choices.count;

        if (charaLeft == 0)
        {
            Debug.LogWarning("Plus de persos basiques disponibles.");
            return;
        }
        int randomN = Random.Range(0, charaLeft);
        AddCharacter(choices, randomN);
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

    
}
