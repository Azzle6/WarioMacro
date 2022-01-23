using GameTypes;
using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "MacroGame/Character", order = 0)]
// ReSharper disable once CheckNamespace
public class Character : ScriptableObject
{
    public enum Level
    {
        Novice, Expert
    }
    [GameType(typeof(CharacterType))]
    public int characterType = 1;
    public Level mastery = Level.Expert;
    public Sprite cardSprite;
    public Sprite lifebarSprite;
}