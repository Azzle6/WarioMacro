using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "newMiniGameSO", menuName = "MacroGame/MinigameSO", order = 1)]
public class MiniGameScriptableObject : ScriptableObject
{
    public enum ButtonsNames
    {
        ABXY,
        LeftJoystick,
        RightJoystick,
        LeftTrigger,
        RightTrigger,
        LeftButton,
        RightButton,
        DirectionalButtons
    }

    public ButtonsNames[] MiniGameInput;
    public string MiniGameKeyword;
    public SceneAsset MiniGameScene;
    
}
