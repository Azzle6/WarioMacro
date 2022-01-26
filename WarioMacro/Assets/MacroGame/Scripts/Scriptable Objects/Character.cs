using GameTypes;
using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "MacroGame/Character", order = 0)]
// ReSharper disable once CheckNamespace
public class Character : ScriptableObject
{
    [GameType(typeof(SpecialistType))]
    public int characterType = 2;
    public enum Level
    {
        Novice, Expert
    }
    public Level mastery = Level.Expert;
    public Sprite cardSprite;
    public Sprite lifebarSprite;
    public Sprite portraitSprite;
    public GameObject PuppetPrefab;

}