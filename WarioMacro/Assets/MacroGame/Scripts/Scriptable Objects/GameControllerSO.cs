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
    [Header("Neutral Node :")]
    [Range(1, 5)] public int defaultMGCount = 3;
    [Range(0, 5)] public int defaultIncreaseDifficultyThreshold = 2;
    [Range(0, 5)] public int defaultDecreaseDifficultyThreshold = 2;
    [Range(0, 5)] public int defaultLoseCharacterThreshold = 0;
    [Space]
    [Header("Typed Node with a specialist in your team :")]
    [Range(1, 5)] public int specialistMGCount = 3;
    [Range(0, 5)] public int specialistIncreaseDifficultyThreshold = 2;
    [Range(0, 5)] public int specialistDecreaseDifficultyThreshold = 2;
    [Range(0, 5)] public int specialistLoseCharacterThreshold = 0;
    [Space]
    [Header("Typed Node with no specialists in your team :")]
    [Range(1, 5)] public int noSpecialistMGCount = 5;
    [Range(0, 5)] public int noSpecialistIncreaseDifficultyThreshold = 3;
    [Range(0, 5)] public int noSpecialistDecreaseDifficultyThreshold = 3;
    [Range(0, 5)] public int noSpecialistLoseCharacterThreshold = 2;
}
