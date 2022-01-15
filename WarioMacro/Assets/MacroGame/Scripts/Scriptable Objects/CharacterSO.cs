using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "MacroGame/Character", order = 0)]
public class CharacterSO : ScriptableObject
{
    public CharaTypes CharacterType;
    
    public Sprite CardSprite;
}

public enum CharaTypes
{
    Brute, Alchemist, Expert, Ghost, Acrobat, Technomancer, Scoundrel
}