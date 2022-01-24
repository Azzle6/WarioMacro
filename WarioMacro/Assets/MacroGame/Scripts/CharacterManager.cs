using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

// ReSharper disable once CheckNamespace
public class CharacterManager : MonoBehaviour
{
    public static CharacterManager instance;
    public List<Character> playerTeam = new List<Character>();
    public List<Character> recruitableCharacters = new List<Character>();
    public CharacterList[] allAvailableCharacters = new CharacterList[6];
    public Character[] novices = new Character[6];
    public List<Imprisoned> imprisonedCharacters = new List<Imprisoned>();
    
    public delegate void RecruitCharacter();
    public static RecruitCharacter RecruitableCharaFinished;

    private void Awake()
    {
        if (instance != null) return;
        instance = this;
    }

    public int SpecialistOfTypeInTeam(int type)
    {
        return playerTeam.Count(c => c.characterType == type);
    }
    [Serializable]
    public class Imprisoned
    {
        public Character character;
        public float turnLeft;
        public float price;

        public Imprisoned(Character character1, int i, int i1)
        {
            character=character1;
            turnLeft = i;
            price = i1;
        }
    }
    
    private void Start()
    {
        LoadAvailable();
        SetRecruitable();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LoseCharacter();
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            UpdateAvailable();
        }
    }

    private void SetRecruitable()
    {
        foreach (var list in allAvailableCharacters)
        {
            var rand = Random.Range(0, 2);
            if (list.count >= 2)
            {
                recruitableCharacters.Add(list.Get(rand));
                list.RemoveAt(rand);
            }
            else if (list.count == 1)
            {
                recruitableCharacters.Add(rand == 0 ? list.Get(rand) : novices.First(t => t.characterType == list.type)); 
            }
            else
            {
                recruitableCharacters.Add(novices.First(t => t.characterType == list.type));
            }
        }

        RecruitableCharaFinished();
    }


    public void Recruit(Character character)
    {
        playerTeam.Add(character);
        recruitableCharacters.Remove(character);
    }

    
    public void LoseCharacter()
    {
        var rand = Random.Range(0, 4);
        imprisonedCharacters.Add(new Imprisoned(playerTeam[rand],3,25000));
        playerTeam.Remove(playerTeam[rand]);
    }

    public void LoadAvailable()
    {
        var temp = PlayerPrefs.GetString("imprisoned");
        if (temp == "") return;
        var imprisoned = temp.Split(';');
        Character tempC = null;
        bool canRemove = false;
        foreach (var item in imprisoned)
        {
            var temp2 = item.Split(',');
            foreach (var list in allAvailableCharacters)
            {
                foreach (var character in list)
                {
                    if (character.ToString() != temp2[0]) continue;
                    tempC = character;
                    canRemove = true;
                }
                if (!canRemove) continue;
                imprisonedCharacters.Add(new Imprisoned(tempC,int.Parse(temp2[1]),int.Parse(temp2[2])));
                list.Remove(tempC);
                canRemove = false;
            }
        }
    }

    public void UpdateImprisoned()
    {
        foreach (var imprisoned in imprisonedCharacters)
        {
            imprisoned.turnLeft--;
            if (imprisoned.turnLeft != 0) continue;
            allAvailableCharacters.First(l => l.type == imprisoned.character.characterType).Add(imprisoned.character);
            imprisonedCharacters.Remove(imprisoned);
        }
    }
    
    public void UpdateAvailable()
    {
        var save = "";
        PlayerPrefs.DeleteKey("imprisoned");
        foreach (var imprisoned in imprisonedCharacters)
        {
            save += imprisoned.character + "," + imprisoned.turnLeft + "," + imprisoned.price +
                    ";";
        }
        save = save.Substring(0, save.Length - 1);
        PlayerPrefs.SetString("imprisoned",save);
    }
    
    public void ResetList()
    {
        foreach (var c in playerTeam)
        {
            if (imprisonedCharacters.Any(i =>i.character == c)) continue;
            foreach (var list in allAvailableCharacters.Where(list => c.characterType == list.type))
            {
                list.Add(c);
            }
        }
        playerTeam.Clear();
        SetRecruitable();
    }

    public void FreeImprisoned(Imprisoned imp)
    {
        if (!GameController.instance.scoreManager.Pay((int)imp.price)) return;
        foreach (var list in allAvailableCharacters.Where(list => imp.character.characterType == list.type))
        {
            list.Add(imp.character);
            imprisonedCharacters.Remove(imp);
        }
    }

    public Character GetCharacter(string character)
    {
        foreach (var list in allAvailableCharacters)
        {
            foreach (var c in list)
            {
                if (c.ToString() == character)
                {
                    return c;
                }
            }
        }
        return (from i in imprisonedCharacters where i.character.ToString() == character select i.character).FirstOrDefault();
    }
    /*public CharacterList[] allAvailableCharacters;
    public int totalCharacterCount = 4;
    [HideInInspector] public bool isTeamFull;

    [SerializeField] private GameObject recruitmentPanelGO;
    [SerializeField] private Transform buttonsParent;
    [SerializeField] private RecrutementCardPannel_UI recruitmentPanel;
    [SerializeField] private LifeBar life;
    
    public readonly Stack<Character> playerTeam = new Stack<Character>();
    private GameObject[] buttonGOList;
    private int currentCount;

    

    public bool IsTypeAvailable(int type) => allAvailableCharacters.First(list => list.type == type).count != 0;

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

        for (int i = 0; i < choicesCount; i++)
        {
            var i1 = i;
            recruitmentPanel.ShowCharacterCard(delegate { AddCharacter(choices, i1); }, choices.Get(i), i);
        }

        yield return WaitForTeamChange();
        AudioManager.MacroPlaySound("CharacterSelection", 0);
    }

    public IEnumerator AddDifferentSpecialist(int type)
    {
        var choices = allAvailableCharacters
            .Where(cList => cList.type != type && !cList.IsEmpty()).ToList();
        choices.RemoveAt(0);

        CharacterList charaList = choices[Random.Range(0, choices.Count)];
        
        ResetUI();
        
        //Re-positionnement automatique
        recruitmentPanelGO.transform.localPosition = new Vector3(0, 0, 0);

        int rd = Random.Range(0, charaList.count);
        recruitmentPanel.ShowCharacterCard(delegate { AddCharacter(charaList, rd); }, charaList.Get(rd), 0);

        yield return WaitForTeamChange();
    }

    /*public IEnumerator AddDefaultCharacter()
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
        recruitmentPanelGO.transform.localPosition = new Vector3(0, 0, 0);

        recruitmentPanel.ShowCharacterCard(delegate { AddCharacter(choices, randomN); }, choices.Get(randomN), 0);

        yield return WaitForTeamChange();
    }#1#

    private void ResetUI()
    {
        recruitmentPanelGO.SetActive(true);

        foreach (GameObject go in buttonGOList)
        {
            go.SetActive(false);
        }
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
        life.RecruitCharacter(characterList.Get(index));
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
    }*/
}
