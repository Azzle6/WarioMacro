using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[CreateAssetMenu(fileName = "newMiniGameSO", menuName = "MacroGame/MinigameSO", order = 1)]
public class MiniGameScriptableObject : ScriptableObject
{
    public enum ButtonName
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

    public ButtonName[] MiniGameInput;
    public string MiniGameKeyword;
#if UNITY_EDITOR
    public SceneAsset MiniGameScene;
#endif
    public string MiniGameSceneName;
    
}
