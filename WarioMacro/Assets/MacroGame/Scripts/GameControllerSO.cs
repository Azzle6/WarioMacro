using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newGameControllerSO", menuName = "MacroGame/GameControllerSO", order = 0)]
public class GameControllerSO : ScriptableObject
{
    [Range(120f, 190f)]
    public float currentGameSpeed = 120;

    [Range(1, 3)]
    public int currentDifficulty = 1;

}
