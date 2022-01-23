using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameTypes;
using UnityEngine;

[Serializable]
public class CharacterList : IEnumerable<Character>
{
    [GameType(typeof(CharacterType))] public int type = 1;
    public int count => characters.Count;
    //public int exp => characters.ForEach(t => t.mastery == Character.Level.Expert);
    
    [SerializeField] private List<Character> characters;

    public Character Get(int i) => characters[i];

    public void Add(Character character) => characters.Add(character);
    public void RemoveAt(int index) => characters.RemoveAt(index);

    public bool IsEmpty() => count == 0;

    public IEnumerator<Character> GetEnumerator()
    {
        return characters.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
