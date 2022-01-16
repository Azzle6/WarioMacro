using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "MacroGame/Character", order = 0)]
// ReSharper disable once CheckNamespace
public class Character : ScriptableObject
{
    public int characterType;
    
    public Sprite cardSprite;
}