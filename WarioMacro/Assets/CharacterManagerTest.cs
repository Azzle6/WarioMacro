using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class CharacterManagerTest : MonoBehaviour
{

    public Character[] playerTeam = new Character[4];
    public Character[] recruitableCharacters = new Character[6];
    public CharacterList[] allAvailableCharacters = new CharacterList[6];
    public Character[] scoundrels = new Character[6];
    public List<Character> imprisonedCharacters = new List<Character>();

    private int currentCharacterCount = 0;
    private void Start()
    {
        SetRecruitable();
    }

    private void SetRecruitable()
    {
        var i = 0;
        foreach (var list in allAvailableCharacters)
        {
            var rand = Random.Range(0, 2);
            if (list.count > 2)
            {
                recruitableCharacters[i] = list.Get(rand);
                list.RemoveAt(rand);
                i++;
                continue;
            }
            else
            {
               recruitableCharacters[i] = rand == 0 ? list.Get(rand) : foreach (var s in scoundrels.Where(s =>
                                                                                    s.characterType == list.type)) ;
            }
            
            
            
        }
    }


    public void Recruit(Character character)
    {
        playerTeam[currentCharacterCount] = character;
        currentCharacterCount++;
    }

    
    public void LoseCharacter()
    {
        imprisonedCharacters.Add(playerTeam[Random.Range(0,playerTeam.Length)]);
    }

    
    
    
    public void ResetList()
    {
        foreach (var c in playerTeam)
        {
            if (imprisonedCharacters.Contains(c)) continue;
            foreach (var list in allAvailableCharacters.Where(list => c.characterType == list.type))
            {
                list.Add(c);
            }
        }
        playerTeam = null;
        currentCharacterCount = 0;
        SetRecruitable();
    }
}
