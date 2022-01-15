using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MicroGameData", menuName = "ScriptableObjects/MicroGameData/NAA_ChainedFistData", order = 2)]
public class NAA_MicroGameData_LAC : ScriptableObject
{
    [Header("Bpm")]
    [Range(0.1f,2)]
    public float baseBpmMult;
    [Header("Difficulty")]
    public DifficultySettings[] difficultySettings;

    [System.Serializable]
    public struct DifficultySettings
    {
        [Range(0.1f, 2)]
        public float BpmMult;
        [Range(0, 1)]
        public float changeDirPrbPerTick;
    }
}
