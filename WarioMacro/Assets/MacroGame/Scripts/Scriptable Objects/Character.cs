using GameTypes;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Character", menuName = "MacroGame/Character", order = 0)]
// ReSharper disable once CheckNamespace
public class Character : ScriptableObject
{
    [GameType(typeof(SpecialistType))]
    public int characterType = 2;
    
    public Level mastery = Level.Expert;
    public Gender gender;
    public string characterName;
    
    [FormerlySerializedAs("portrait")] 
    [FormerlySerializedAs("cardSprite")] 
    public Sprite fullSizeSprite;
    [FormerlySerializedAs("lifebarSprite")] public Sprite portraitSprite;
    
    public GameObject PuppetPrefab;

    public enum Level
    {
        Novice, Expert
    }
    
    public enum Gender
    {
        Male,
        Female,
        Dog
    }
}