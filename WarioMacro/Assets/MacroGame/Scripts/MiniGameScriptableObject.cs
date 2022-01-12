using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "newMiniGameSO", menuName = "MacroGame/MinigameSO", order = 1)]
public class MiniGameScriptableObject : ScriptableObject
{
    public enum ButtonsNames
    {
        X,
        Y,
        A,
        B,
        LeftJoystick,
        RightJoystick,
        LeftTrigger,
        RightTrigger,
        LeftButton,
        RightButton,
        DirectionalButtons
    }

    public ButtonsNames MiniGameInput;
    public string MiniGameKeyword;
    public Scene MiniGameScene;
}
