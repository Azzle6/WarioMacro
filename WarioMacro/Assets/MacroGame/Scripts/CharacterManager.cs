using System;
using System.Collections;
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

    
    
    [SerializeField] private LifeBar life;

    
    public delegate void RecruitCharacter();
    public static RecruitCharacter RecruitableCharaFinished;
    public static bool IsFirstLoad = true;

    private void Awake()
    {
        RecruitableCharaFinished = null;
        if (instance != null) return;
        instance = this;
    }


    public Character SpecialistOfTypeInTeam(int type)
    {
        return playerTeam.FirstOrDefault(c => c.characterType == type);
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
        if (IsFirstLoad)
        {
            GameController.instance.hallOfFame.SetHallOfFame();
            LoadAvailable();
            SetRecruitable();
        }
    }
    

    
    
    private void SetRecruitable()
    {
        StartCoroutine(SetRecruitWithTiming());
    }

    private IEnumerator SetRecruitWithTiming()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        recruitableCharacters = new List<Character>();
        
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
                if (rand == 1)
                    recruitableCharacters.Add(novices.First(t => t.characterType == list.type));
                else
                {
                    recruitableCharacters.Add(list.Get(rand));
                    list.RemoveAt(rand);
                }
            }
            else
            {
                recruitableCharacters.Add(novices.First(t => t.characterType == list.type));
            }
        }
        RecruitableCharaFinished();
    }


    public void ResetEndGame()
    {
        Debug.Log("ResetList");
        UpdateImprisoned();
        ResetList();
        UpdateAvailable();
        LoadAvailable();
        SetRecruitable();
    }

    public void Recruit(Character character)
    {
        playerTeam.Add(character);
        life.RecruitCharacter(character);
        recruitableCharacters.Remove(character);
    }

    
    public void LoseCharacter()
    {
        Character rdCharacter = playerTeam[Random.Range(0, playerTeam.Count)];
        life.Imprison(rdCharacter);
        GameController.instance.hallOfFame.SetCharacterToJail(rdCharacter);
        imprisonedCharacters.Add(new Imprisoned(rdCharacter, 4, 3000));
        playerTeam.Remove(rdCharacter);
        UpdateAvailable();
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
        for (int i = imprisonedCharacters.Count - 1; i >= 0; i--)
        {
            var imprisoned = imprisonedCharacters[i];
            imprisoned.turnLeft--;
            if (imprisoned.turnLeft != 0) continue;
            allAvailableCharacters.First(l => l.type == imprisoned.character.characterType).Add(imprisoned.character);
            imprisonedCharacters.RemoveAt(i);
        }
    }
    
    public void UpdateAvailable()
    {
        PlayerPrefs.DeleteKey("imprisoned");
        if (!imprisonedCharacters.Any()) return;
        var save = "";
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
            if (novices.Any(n => n == c)) continue;
            foreach (var list in allAvailableCharacters.Where(list => c.characterType == list.type))
            {
                list.Add(c);
            }
        }
        playerTeam.Clear();
        //SetRecruitable();
    }
    
    public bool FreeImprisoned(Imprisoned imp)
    {
        if (!imprisonedCharacters.Contains(imp)) return false;
        if (!GameController.instance.scoreManager.Pay((int)imp.price)) return false;
        foreach (var list in allAvailableCharacters.Where(list => imp.character.characterType == list.type))
        {
            list.Add(imp.character);
            imprisonedCharacters.Remove(imp);
        }
        
        UpdateAvailable();
        PlayerPrefs.Save();

        return true;
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

        foreach (var c in novices)
        {
            if (c.ToString() == character)
            {
                return c;
            }
        }
        return (from i in imprisonedCharacters where i.character.ToString() == character select i.character).FirstOrDefault();
    }
}
