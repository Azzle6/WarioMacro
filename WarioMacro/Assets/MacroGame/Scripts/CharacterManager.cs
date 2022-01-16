using System.Collections.Generic;
using System.Linq;
using GameTypes;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

// ReSharper disable once CheckNamespace
public class CharacterManager : MonoBehaviour
{
    public List<Character> allAvailableCharacters = new List<Character>();

    [SerializeField] private GameObject chooseCharacterGO;
    [SerializeField] private GameObject cardButtonTemplate;
    
    private readonly Stack<Character> playerTeam = new Stack<Character>();

    public void DisplayRecrutementChoice(int charaType)
    {
        var choiceList = new List<Character>(allAvailableCharacters.Where(character => character.characterType == charaType));
        chooseCharacterGO.SetActive(true);

        if (choiceList.Count == 0)
        {
            Debug.LogError("Plus de persos "+ charaType +" disponibles.");
            return;
        }
        
        //Re-positionnement automatique
        float spacing = 900 / Mathf.Pow(3, (choiceList.Count - 1));
        chooseCharacterGO.transform.localPosition =
            choiceList.Count > 1 ? new Vector3(-200, 0, 0) : Vector3.zero;
        

        chooseCharacterGO.GetComponent<HorizontalLayoutGroup>().spacing = spacing;

        //Instancier toutes les cartes dont on a besoin
        var displayedGO = choiceList
            .Select(characterSO => Instantiate(cardButtonTemplate, chooseCharacterGO.transform)).ToList();

        Destroy(chooseCharacterGO.transform.GetChild(0).gameObject);

        for (int i = 0; i < choiceList.Count; i++)
        {
            displayedGO[i].GetComponent<Image>().sprite = choiceList[i].cardSprite;

            //Ajoute un listener au bouton pour qu'il ajoute le bon personnage
            int i1 = i;
            displayedGO[i].GetComponent<Button>().onClick
                .AddListener(delegate { AddCharacter(choiceList[i1]); });
        }
    }

    public int SpecialistOfType(int type)
    {
        return playerTeam.Count(c => c.characterType == type);
    }

    public void AddDefaultCharacter()
    {
        var choices = new List<Character>(allAvailableCharacters.Where(availableChara =>
            availableChara.characterType == CharacterType.Scoundrel));

        if (choices.Count == 0)
        {
            Debug.LogWarning("Plus de persos basiques disponibles.");
            return;
        }
        int randomN = Random.Range(0, choices.Count );
        AddCharacter(choices[randomN]);
    }
    
    public void AddCharacter(Character newChara)
    {
        playerTeam.Push(newChara);
        allAvailableCharacters.Remove(newChara);
        Debug.Log("personnage " + newChara + " a été ajouté à l'équipe!");
    }
    
    public void LoseCharacter()
    {
        playerTeam.Pop();
    }

    
}
