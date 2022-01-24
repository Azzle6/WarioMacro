using UnityEngine;

[CreateAssetMenu(fileName = "Game Config", menuName = "MacroGame/Game Config", order = 0)]
public class GameConfig : ScriptableObject
{
    public static GameConfig instance;
    
    [BPMRange] public int currentGameSpeed = 50;
    [Range(1, 3)] public int currentDifficulty = 1;
    [Space]
    [BPMRange] public int initialGameSpeed = 50;
    [Range(1, 3)] public int initialDifficulty = 1;

    [Space] [Header("Astral Path")] 
    [Range(1, 7)] public int astralMGCount = 3;
    [Range(0, 6)] public int loseCharacterThreshold = 1;
    
    [Space] [Header("Node Security Domain")]
    [Range(0f, 100f)] public float nodePrimaryDomainPercentage = 70;
    [Range(0f, 100f)] public float nodeDoubleDomainPercentage = 50;
    [Space]
    [Range(0f, 100f)] public float mgSingleDomainPercentage = 60;
    [Range(0f, 100f)] public float mgPrimaryDomainPercentage = 50;
    [Range(0f, 100f)] public float mgSecondaryDomainPercentage = 35;

    public void Init()
    {
        instance ??= this;
    }
}
