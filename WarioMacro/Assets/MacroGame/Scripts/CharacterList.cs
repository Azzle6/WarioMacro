using System;
using System.Collections;
using System.Collections.Generic;
using GameTypes;
using UnityEngine;

[Serializable]
public class CharacterList : IEnumerable<Character>
{
    [GameType(typeof(SpecialistType))] public int type = 2;
    public int count => characters.Count;
    
    [SerializeField] private List<Character> characters;

    public Character Get(int i) => characters[i];

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
