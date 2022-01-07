using UnityEngine;

[CreateAssetMenu(fileName = "GameControllerSO", menuName = "MacroGame/GameControllerSO", order = 0)]
public class GameControllerSO : ScriptableObject
{
    [Range(100f, 160f)]
    public float currentGameSpeed = 100;

    [Range(1, 3)]
    public int currentDifficulty = 1;

}
