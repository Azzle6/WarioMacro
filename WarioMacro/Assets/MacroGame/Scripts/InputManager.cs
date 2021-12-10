using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum ControllerKey
{
    A,
    B,
    Y,
    X,
    LB,
    RB,
    LEFTSTICK,
    RIGHTSTICK,
    DPAD_LEFT,
    DPAD_RIGHT,
    DPAD_UP,
    DPAD_DOWN
}

public enum ControllerAxis
{
    LEFT_STICK_HORIZONTAL,
    LEFT_STICK_VERTICAL,
    RIGHT_STICK_HORIZONTAL,
    RIGHT_STICK_VERTICAL,
    LEFT_TRIGGER,
    RIGHT_TRIGGER
}

public class InputManager : MonoBehaviour
{
    static InputManager inputManager = null;

    public static void Register()
    {
        if (inputManager == null)
        {
            inputManager = new GameObject("InputManager").AddComponent<InputManager>();
        }
    }

    public static bool GetKey(ControllerKey key)
    {
        if (IsNotdPad(key))
        {
            return Input.GetButton(ToButton(key));
        }
        else
        {
            return
            dPadToBool(key, ButtonState.PRESSED) ||
            dPadToBool(key, ButtonState.DOWN);
        }
    }

    public static bool GetKeyDown(ControllerKey key)
    {
        if (IsNotdPad(key))
        {
            return Input.GetButtonDown(ToButton(key));
        }
        else
        {
            return dPadToBool(key, ButtonState.DOWN);
        }
    }

    public static bool GetKeyUp(ControllerKey key)
    {
        if (IsNotdPad(key))
        {
            return Input.GetButtonUp(ToButton(key));
        }
        else
        {
            return dPadToBool(key, ButtonState.UP);
        }
    }

    public static float GetAxis(ControllerAxis axis)
    {
        return Input.GetAxis(ToAxis(axis));
    }

    public static float GetAxisRaw(ControllerAxis axis)
    {
        return Input.GetAxisRaw(ToAxis(axis));
    }

    static string ToButton(ControllerKey key)
    {
        return key.ToString();
    }

    static string ToAxis(ControllerAxis axis)
    {
        return axis.ToString();
    }

    #region DPAD Management

    static bool dPadToBool(ControllerKey key, ButtonState state)
    {
        switch (key)
        {
            case ControllerKey.DPAD_LEFT:
                return leftDpadState == state;

            case ControllerKey.DPAD_RIGHT:
                return rightDpadState == state;

            case ControllerKey.DPAD_UP:
                return upDpadState == state;

            case ControllerKey.DPAD_DOWN:
                return downDpadState == state;

            default:
                return false;
        }
    }

    static bool IsNotdPad(ControllerKey key)
    {
        return
        key != ControllerKey.DPAD_LEFT &&
        key != ControllerKey.DPAD_RIGHT &&
        key != ControllerKey.DPAD_UP &&
        key != ControllerKey.DPAD_DOWN;
    }

    static Vector2 lastdPadAxis;
    static Vector2 dPadAxis;

    enum ButtonState
    {
        NONE,
        DOWN,
        UP,
        PRESSED
    }

    static ButtonState leftDpadState = ButtonState.NONE;
    static ButtonState rightDpadState = ButtonState.NONE;
    static ButtonState upDpadState = ButtonState.NONE;
    static ButtonState downDpadState = ButtonState.NONE;

    static float deadZone = 0.2f;

    void Update()
    {
        dPadAxis.x = Input.GetAxis("DpadHorizontal");
        dPadAxis.y = Input.GetAxis("DpadVertical");

        //left
        leftDpadState = dPadAxis.x < -deadZone ?
        (dPadAxis.x != lastdPadAxis.x ? ButtonState.DOWN : ButtonState.PRESSED) :
        ((dPadAxis.x != lastdPadAxis.x && lastdPadAxis.x < -deadZone) ? ButtonState.UP : ButtonState.NONE);

        //right
        rightDpadState = dPadAxis.x > deadZone ?
        (dPadAxis.x != lastdPadAxis.x ? ButtonState.DOWN : ButtonState.PRESSED) :
        ((dPadAxis.x != lastdPadAxis.x && lastdPadAxis.x > deadZone) ? ButtonState.UP : ButtonState.NONE);

        //up
        upDpadState = dPadAxis.y > deadZone ?
        (dPadAxis.y != lastdPadAxis.y ? ButtonState.DOWN : ButtonState.PRESSED) :
        ((dPadAxis.y != lastdPadAxis.y && lastdPadAxis.y > deadZone) ? ButtonState.UP : ButtonState.NONE);

        //down
        downDpadState = dPadAxis.y < -deadZone ?
        (dPadAxis.y != lastdPadAxis.y ? ButtonState.DOWN : ButtonState.PRESSED) :
        ((dPadAxis.y != lastdPadAxis.y && lastdPadAxis.y < -deadZone) ? ButtonState.UP : ButtonState.NONE);


        lastdPadAxis = dPadAxis;

    }

    #endregion

    #region Bruteforce InputManager.asset

#if UNITY_EDITOR
    [UnityEditor.MenuItem("Tools/Update Input Manager Asset")]
#endif
    static public void UpdateInputManagerAsset()
    {
        var filepath = System.IO.Directory.GetParent(Application.dataPath) + "/ProjectSettings/InputManager.asset";

        var newtext = @"%YAML 1.1
%TAG !u! tag:unity3d.com,2011:
--- !u!13 &1
InputManager:
  m_ObjectHideFlags: 0
  serializedVersion: 2
  m_Axes:
  - serializedVersion: 3
    m_Name: LEFT_STICK_HORIZONTAL
    descriptiveName: 
    descriptiveNegativeName: 
    negativeButton: 
    positiveButton: 
    altNegativeButton: 
    altPositiveButton: 
    gravity: 0
    dead: 0.19
    sensitivity: 1
    snap: 1
    invert: 0
    type: 2
    axis: 0
    joyNum: 0
  - serializedVersion: 3
    m_Name: LEFT_STICK_VERTICAL
    descriptiveName: 
    descriptiveNegativeName: 
    negativeButton: 
    positiveButton: 
    altNegativeButton: 
    altPositiveButton: 
    gravity: 0
    dead: 0.19
    sensitivity: 1
    snap: 1
    invert: 1
    type: 2
    axis: 1
    joyNum: 0
  - serializedVersion: 3
    m_Name: A
    descriptiveName: 
    descriptiveNegativeName: 
    negativeButton: 
    positiveButton: joystick button 0
    altNegativeButton: 
    altPositiveButton: 
    gravity: 1000
    dead: 0.001
    sensitivity: 1000
    snap: 0
    invert: 0
    type: 0
    axis: 0
    joyNum: 0
  - serializedVersion: 3
    m_Name: B
    descriptiveName: 
    descriptiveNegativeName: 
    negativeButton: 
    positiveButton: joystick button 1
    altNegativeButton: 
    altPositiveButton: 
    gravity: 1000
    dead: 0.001
    sensitivity: 1000
    snap: 0
    invert: 0
    type: 0
    axis: 0
    joyNum: 0
  - serializedVersion: 3
    m_Name: Y
    descriptiveName: 
    descriptiveNegativeName: 
    negativeButton: 
    positiveButton: joystick button 3
    altNegativeButton: 
    altPositiveButton: 
    gravity: 1000
    dead: 0.001
    sensitivity: 1000
    snap: 0
    invert: 0
    type: 0
    axis: 0
    joyNum: 0
  - serializedVersion: 3
    m_Name: X
    descriptiveName: 
    descriptiveNegativeName: 
    negativeButton: 
    positiveButton: joystick button 2
    altNegativeButton: 
    altPositiveButton: 
    gravity: 1000
    dead: 0.001
    sensitivity: 1000
    snap: 0
    invert: 0
    type: 0
    axis: 0
    joyNum: 0
  - serializedVersion: 3
    m_Name: LB
    descriptiveName: 
    descriptiveNegativeName: 
    negativeButton: 
    positiveButton: joystick button 4
    altNegativeButton: 
    altPositiveButton: 
    gravity: 1000
    dead: 0.001
    sensitivity: 1000
    snap: 0
    invert: 0
    type: 0
    axis: 0
    joyNum: 0
  - serializedVersion: 3
    m_Name: RB
    descriptiveName: 
    descriptiveNegativeName: 
    negativeButton: 
    positiveButton: joystick button 5
    altNegativeButton: 
    altPositiveButton: 
    gravity: 1000
    dead: 0.001
    sensitivity: 1000
    snap: 0
    invert: 0
    type: 0
    axis: 0
    joyNum: 0
  - serializedVersion: 3
    m_Name: LEFTSTICK
    descriptiveName: 
    descriptiveNegativeName: 
    negativeButton: 
    positiveButton: joystick button 8
    altNegativeButton: 
    altPositiveButton: 
    gravity: 1000
    dead: 0.001
    sensitivity: 1000
    snap: 0
    invert: 0
    type: 0
    axis: 0
    joyNum: 0
  - serializedVersion: 3
    m_Name: RIGHTSTICK
    descriptiveName: 
    descriptiveNegativeName: 
    negativeButton: 
    positiveButton: joystick button 9
    altNegativeButton: 
    altPositiveButton: 
    gravity: 1000
    dead: 0.001
    sensitivity: 1000
    snap: 0
    invert: 0
    type: 0
    axis: 0
    joyNum: 0
  - serializedVersion: 3
    m_Name: RIGHT_STICK_HORIZONTAL
    descriptiveName: 
    descriptiveNegativeName: 
    negativeButton: 
    positiveButton: 
    altNegativeButton: 
    altPositiveButton: 
    gravity: 0
    dead: 0.19
    sensitivity: 1
    snap: 1
    invert: 0
    type: 2
    axis: 3
    joyNum: 0
  - serializedVersion: 3
    m_Name: RIGHT_STICK_VERTICAL
    descriptiveName: 
    descriptiveNegativeName: 
    negativeButton: 
    positiveButton: 
    altNegativeButton: 
    altPositiveButton: 
    gravity: 0
    dead: 0.19
    sensitivity: 1
    snap: 1
    invert: 1
    type: 2
    axis: 4
    joyNum: 0
  - serializedVersion: 3
    m_Name: DpadHorizontal
    descriptiveName: 
    descriptiveNegativeName: 
    negativeButton: 
    positiveButton: 
    altNegativeButton: 
    altPositiveButton: 
    gravity: 0
    dead: 0.19
    sensitivity: 1
    snap: 1
    invert: 0
    type: 2
    axis: 5
    joyNum: 0
  - serializedVersion: 3
    m_Name: DpadVertical
    descriptiveName: 
    descriptiveNegativeName: 
    negativeButton: 
    positiveButton: 
    altNegativeButton: 
    altPositiveButton: 
    gravity: 0
    dead: 0.19
    sensitivity: 1
    snap: 1
    invert: 0
    type: 2
    axis: 6
    joyNum: 0
  - serializedVersion: 3
    m_Name: LEFT_TRIGGER
    descriptiveName: 
    descriptiveNegativeName: 
    negativeButton: 
    positiveButton: 
    altNegativeButton: 
    altPositiveButton: 
    gravity: 0
    dead: 0.05
    sensitivity: 1
    snap: 0
    invert: 0
    type: 2
    axis: 8
    joyNum: 0
  - serializedVersion: 3
    m_Name: RIGHT_TRIGGER
    descriptiveName: 
    descriptiveNegativeName: 
    negativeButton: 
    positiveButton: 
    altNegativeButton: 
    altPositiveButton: 
    gravity: 0
    dead: 0.05
    sensitivity: 1
    snap: 0
    invert: 0
    type: 2
    axis: 9
    joyNum: 0
  - serializedVersion: 3
    m_Name: Debug Persistent
    descriptiveName: 
    descriptiveNegativeName: 
    negativeButton: 
    positiveButton: right shift
    altNegativeButton: 
    altPositiveButton: joystick button 2
    gravity: 0
    dead: 0
    sensitivity: 0
    snap: 0
    invert: 0
    type: 0
    axis: 0
    joyNum: 0
  - serializedVersion: 3
    m_Name: Debug Multiplier
    descriptiveName: 
    descriptiveNegativeName: 
    negativeButton: 
    positiveButton: left shift
    altNegativeButton: 
    altPositiveButton: joystick button 3
    gravity: 0
    dead: 0
    sensitivity: 0
    snap: 0
    invert: 0
    type: 0
    axis: 0
    joyNum: 0
  - serializedVersion: 3
    m_Name: Debug Horizontal
    descriptiveName: 
    descriptiveNegativeName: 
    negativeButton: left
    positiveButton: right
    altNegativeButton: 
    altPositiveButton: 
    gravity: 1000
    dead: 0.001
    sensitivity: 1000
    snap: 0
    invert: 0
    type: 0
    axis: 0
    joyNum: 0
  - serializedVersion: 3
    m_Name: Debug Vertical
    descriptiveName: 
    descriptiveNegativeName: 
    negativeButton: down
    positiveButton: up
    altNegativeButton: 
    altPositiveButton: 
    gravity: 1000
    dead: 0.001
    sensitivity: 1000
    snap: 0
    invert: 0
    type: 0
    axis: 0
    joyNum: 0
  - serializedVersion: 3
    m_Name: Debug Vertical
    descriptiveName: 
    descriptiveNegativeName: 
    negativeButton: down
    positiveButton: up
    altNegativeButton: 
    altPositiveButton: 
    gravity: 1000
    dead: 0.001
    sensitivity: 1000
    snap: 0
    invert: 0
    type: 2
    axis: 6
    joyNum: 0
  - serializedVersion: 3
    m_Name: Debug Horizontal
    descriptiveName: 
    descriptiveNegativeName: 
    negativeButton: left
    positiveButton: right
    altNegativeButton: 
    altPositiveButton: 
    gravity: 1000
    dead: 0.001
    sensitivity: 1000
    snap: 0
    invert: 0
    type: 2
    axis: 5
    joyNum: 0
  - serializedVersion: 3
    m_Name: Enable Debug Button 1
    descriptiveName: 
    descriptiveNegativeName: 
    negativeButton: 
    positiveButton: left ctrl
    altNegativeButton: 
    altPositiveButton: joystick button 8
    gravity: 0
    dead: 0
    sensitivity: 0
    snap: 0
    invert: 0
    type: 0
    axis: 0
    joyNum: 0
  - serializedVersion: 3
    m_Name: Enable Debug Button 2
    descriptiveName: 
    descriptiveNegativeName: 
    negativeButton: 
    positiveButton: backspace
    altNegativeButton: 
    altPositiveButton: joystick button 9
    gravity: 0
    dead: 0
    sensitivity: 0
    snap: 0
    invert: 0
    type: 0
    axis: 0
    joyNum: 0
  - serializedVersion: 3
    m_Name: Debug Reset
    descriptiveName: 
    descriptiveNegativeName: 
    negativeButton: 
    positiveButton: left alt
    altNegativeButton: 
    altPositiveButton: joystick button 1
    gravity: 0
    dead: 0
    sensitivity: 0
    snap: 0
    invert: 0
    type: 0
    axis: 0
    joyNum: 0
  - serializedVersion: 3
    m_Name: Debug Next
    descriptiveName: 
    descriptiveNegativeName: 
    negativeButton: 
    positiveButton: page down
    altNegativeButton: 
    altPositiveButton: joystick button 5
    gravity: 0
    dead: 0
    sensitivity: 0
    snap: 0
    invert: 0
    type: 0
    axis: 0
    joyNum: 0
  - serializedVersion: 3
    m_Name: Debug Previous
    descriptiveName: 
    descriptiveNegativeName: 
    negativeButton: 
    positiveButton: page up
    altNegativeButton: 
    altPositiveButton: joystick button 4
    gravity: 0
    dead: 0
    sensitivity: 0
    snap: 0
    invert: 0
    type: 0
    axis: 0
    joyNum: 0
  - serializedVersion: 3
    m_Name: Debug Validate
    descriptiveName: 
    descriptiveNegativeName: 
    negativeButton: 
    positiveButton: return
    altNegativeButton: 
    altPositiveButton: joystick button 0
    gravity: 0
    dead: 0
    sensitivity: 0
    snap: 0
    invert: 0
    type: 0
    axis: 0
    joyNum: 0
";

        System.IO.File.WriteAllText(filepath, newtext);
    }

    #endregion
}
