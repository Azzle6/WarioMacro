using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;


public class CharacterManagerTest : MonoBehaviour
{

    public List<Character> playerTeam = new List<Character>();
    public List<Character> recruitableCharacters = new List<Character>();
    public CharacterList[] allAvailableCharacters = new CharacterList[6];
    public Character[] scoundrels = new Character[6];
    public List<Imprisoned> imprisonedCharacters = new List<Imprisoned>();
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
        for (int i = 0; i < 4; i++)
        {
            Recruit(recruitableCharacters[Random.Range(0,recruitableCharacters.Count)]);
        }
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
                recruitableCharacters.Add(rand == 0 ? list.Get(rand) : scoundrels.First(t => t.characterType == list.type)); 
            }
            else
            {
                recruitableCharacters.Add(scoundrels.First(t => t.characterType == list.type));
            }
        }
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
}
