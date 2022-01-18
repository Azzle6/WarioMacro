using UnityEngine;

[CreateAssetMenu(fileName = "GameControllerSO", menuName = "MacroGame/GameControllerSO", order = 0)]
public class GameControllerSO : ScriptableObject
{
    [BPMRange] public int currentGameSpeed = 50;
    [Range(1, 3)] public int currentDifficulty = 1;
    [Space]
    [BPMRange] public int initialGameSpeed = 50;
    [Range(1, 3)] public int initialDifficulty = 1;
    [Space]
    [Range(1, 5)] public int defaultMGCount = 3;
    [Range(0, 5)] public int increaseDifficultyThreshold = 2;
    [Range(0, 5)] public int decreaseDifficultyThreshold = 2;
    [Range(0, 5)] public int loseCharacterThreshold = 0;
    [Space]
    [Range(1, 5)] public int noSpecialistMGCount = 5;
    [Range(0, 5)] public int noSpecialistIncreaseDifficultyThreshold = 3;
    [Range(0, 5)] public int noSpecialistDecreaseDifficultyThreshold = 3;
    [Range(0, 5)] public int noSpecialistLoseCharacterThreshold = 2;
}
