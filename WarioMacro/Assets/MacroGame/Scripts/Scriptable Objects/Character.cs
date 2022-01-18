using GameTypes;
using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "MacroGame/Character", order = 0)]
// ReSharper disable once CheckNamespace
public class Character : ScriptableObject
{
    [GameType(typeof(CharacterType))]
    public int characterType = 1;
    
    public Sprite cardSprite;
    public Sprite lifebarSprite;
}