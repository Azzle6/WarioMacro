using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CharactersManager : MonoBehaviour
{
    public static CharactersManager instance;

    public List<CharacterSO> AllAvailableCharacters = new List<CharacterSO>();

    public List<CharacterSO> PlayerCurrentTeam = new List<CharacterSO>();

    [SerializeField] private GameObject ChooseCharacterGO;
    [SerializeField] private GameObject CardButtonTemplate;
    [SerializeField] private List<CharacterSO> currentChooseList;

    private void Start()
    {
        instance = this;
        DisplayCharacters(CharaTypes.Alchemist);
    }

    public void DisplayCharacters(CharaTypes charType)
    {
        currentChooseList = new List<CharacterSO>();
        ChooseCharacterGO.SetActive(true);
        foreach (CharacterSO availablesChara in AllAvailableCharacters)
        {
            if(availablesChara.CharacterType == charType) currentChooseList.Add(availablesChara);
        }
        
        if (currentChooseList.Count == 0)
        {
            Debug.Log("Plus de persos "+ charType +" disponibles.");
            return;
        }
        
        //Repositionnement automatique
        float spacing = 900 / Mathf.Pow(3, (currentChooseList.Count - 1));
        if (currentChooseList.Count > 1)
        {
            ChooseCharacterGO.transform.localPosition = new Vector3(-200, 0, 0);
        }
        else ChooseCharacterGO.transform.localPosition = Vector3.zero;
        

        ChooseCharacterGO.GetComponent<HorizontalLayoutGroup>().spacing = spacing;

        //Instancier toutes les cartes dont on a besoin
        List<GameObject> DisplayedGO = new List<GameObject>();
        
        foreach (var VARIABLE in currentChooseList)
        {
            GameObject go = Instantiate(CardButtonTemplate, ChooseCharacterGO.transform);
            DisplayedGO.Add(go);
        }
        Destroy(ChooseCharacterGO.transform.GetChild(0).gameObject);

        for (int i = 0; i < currentChooseList.Count; i++)
        {
            DisplayedGO[i].GetComponent<Image>().sprite = currentChooseList[i].CardSprite;

            //Ajoute un listener au bouton pour qu'il ajoute le bon personnage
            int i1 = i;
            DisplayedGO[i].GetComponent<Button>().onClick.AddListener(delegate { AddCharacter(currentChooseList[i1]);});
        }
    }

    public void AddbasicCharacter()
    {
        currentChooseList = new List<CharacterSO>();
        foreach (CharacterSO availablesChara in AllAvailableCharacters)
        {
            if(availablesChara.CharacterType == CharaTypes.Scoundrel) currentChooseList.Add(availablesChara);
        }

        if (currentChooseList.Count == 0)
        {
            Debug.Log("Plus de persos basiques disponibles.");
            return;
        }
        int randomN = Random.Range(0, currentChooseList.Count );
        AddCharacter(currentChooseList[randomN]);
    }
    
    public void AddCharacter(CharacterSO newChara)
    {
        instance.PlayerCurrentTeam.Add(newChara);
        Debug.Log("personnage " + newChara.ToString() + " a été ajouté à l'équipe!");
        for (int i = 0; i < AllAvailableCharacters.Count; i++)
        {
            if (AllAvailableCharacters[i] == newChara)
            {
                AllAvailableCharacters.RemoveAt(i);
            }
        }
    }

    public void LoseCharacter()
    {
        PlayerCurrentTeam.RemoveAt(PlayerCurrentTeam.Count - 1);
    }
}
