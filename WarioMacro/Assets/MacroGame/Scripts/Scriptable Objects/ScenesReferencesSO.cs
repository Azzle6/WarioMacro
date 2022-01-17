using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CreateAssetMenu(fileName = "ScenesReferences", menuName = "MacroGame/ScenesReferences", order = 0)]
public class ScenesReferencesSO : ScriptableObject
{
    public MiniGameScriptableObject[] MiniGames;
}
