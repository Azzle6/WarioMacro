using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEditor;
using UnityEngine.EventSystems;

// ReSharper disable InconsistentNaming

// ReSharper disable once CheckNamespace
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
    private static InputManager inputManager;

    public static void Register()
    {
        if (inputManager != null) return;
        
        inputManager = new GameObject("InputManager").AddComponent<InputManager>();
        inputManager.gameObject.AddComponent<EventSystemBehaviour>();
    }

    public static bool GetKey(ControllerKey key)
    {
        if (IsNotDPad(key))
        {
            return Input.GetButton(ToButton(key));
        }

        return
            DPadToBool(key, ButtonState.PRESSED) ||
            DPadToBool(key, ButtonState.DOWN);
    }

    public static bool GetKeyDown(ControllerKey key)
    {
        return IsNotDPad(key) ? Input.GetButtonDown(ToButton(key)) : DPadToBool(key, ButtonState.DOWN);
    }

    public static bool GetKeyUp(ControllerKey key)
    {
        return IsNotDPad(key) ? Input.GetButtonUp(ToButton(key)) : DPadToBool(key, ButtonState.UP);
    }

    public static float GetAxis(ControllerAxis axis)
    {
        return Input.GetAxis(ToAxis(axis));
    }

    public static float GetAxisRaw(ControllerAxis axis)
    {
        return Input.GetAxisRaw(ToAxis(axis));
    }

    public static MoveDirection GetDirection()
    {
        float horizontalInput = GetAxis(ControllerAxis.LEFT_STICK_HORIZONTAL);
        float verticalInput = GetAxis(ControllerAxis.LEFT_STICK_VERTICAL);
        
        if (horizontalInput < 0.5f && horizontalInput > -0.5f && verticalInput < 0.5f && verticalInput > -0.5f)
            return MoveDirection.None;
        
        Debug.Log(verticalInput);
        float joystickRotation =
            Mathf.Atan2(horizontalInput, verticalInput) * 180 / Mathf.PI;

        if (joystickRotation > -45 && joystickRotation <= 45)
            return MoveDirection.Up;
        if (joystickRotation > 45 && joystickRotation <= 135) 
            return MoveDirection.Right;
        if (joystickRotation > -135 && joystickRotation <= -45) 
            return MoveDirection.Left;
        
        return MoveDirection.Down;
    }

    private static string ToButton(ControllerKey key)
    {
        return key.ToString();
    }

    private static string ToAxis(ControllerAxis axis)
    {
        return axis.ToString();
    }

    #region DPAD Management

    private static bool DPadToBool(ControllerKey key, ButtonState state)
    {
        return key switch
        {
            ControllerKey.DPAD_LEFT => leftDpadState == state,
            ControllerKey.DPAD_RIGHT => rightDpadState == state,
            ControllerKey.DPAD_UP => upDpadState == state,
            ControllerKey.DPAD_DOWN => downDpadState == state,
            _ => false
        };
    }

    private static bool IsNotDPad(ControllerKey key)
    {
        return
        key != ControllerKey.DPAD_LEFT &&
        key != ControllerKey.DPAD_RIGHT &&
        key != ControllerKey.DPAD_UP &&
        key != ControllerKey.DPAD_DOWN;
    }

    private static Vector2 lastDPadAxis;
    private static Vector2 dPadAxis;

    private enum ButtonState
    {
        NONE,
        DOWN,
        UP,
        PRESSED
    }

    private static ButtonState leftDpadState = ButtonState.NONE;
    private static ButtonState rightDpadState = ButtonState.NONE;
    private static ButtonState upDpadState = ButtonState.NONE;
    private static ButtonState downDpadState = ButtonState.NONE;

    private const float deadZone = 0.2f;

    [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
    private void Update()
    {
        dPadAxis.x = Input.GetAxis("DpadHorizontal");
        dPadAxis.y = Input.GetAxis("DpadVertical");

        //left
        leftDpadState = dPadAxis.x < -deadZone ?
        (dPadAxis.x != lastDPadAxis.x ? ButtonState.DOWN : ButtonState.PRESSED) :
        ((dPadAxis.x != lastDPadAxis.x && lastDPadAxis.x < -deadZone) ? ButtonState.UP : ButtonState.NONE);

        //right
        rightDpadState = dPadAxis.x > deadZone ?
        (dPadAxis.x != lastDPadAxis.x ? ButtonState.DOWN : ButtonState.PRESSED) :
        ((dPadAxis.x != lastDPadAxis.x && lastDPadAxis.x > deadZone) ? ButtonState.UP : ButtonState.NONE);

        //up
        upDpadState = dPadAxis.y > deadZone ?
        (dPadAxis.y != lastDPadAxis.y ? ButtonState.DOWN : ButtonState.PRESSED) :
        ((dPadAxis.y != lastDPadAxis.y && lastDPadAxis.y > deadZone) ? ButtonState.UP : ButtonState.NONE);

        //down
        downDpadState = dPadAxis.y < -deadZone ?
        (dPadAxis.y != lastDPadAxis.y ? ButtonState.DOWN : ButtonState.PRESSED) :
        ((dPadAxis.y != lastDPadAxis.y && lastDPadAxis.y < -deadZone) ? ButtonState.UP : ButtonState.NONE);


        lastDPadAxis = dPadAxis;

    }

    #endregion

    #region Bruteforce InputManager.asset

#if UNITY_EDITOR
    [MenuItem("Tools/Update Input Manager Asset")]
#endif
    public static void UpdateInputManagerAsset()
    {
        var filepath = System.IO.Directory.GetParent(Application.dataPath) + "/ProjectSettings/InputManager.asset";

        const string newText = @"%YAML 1.1
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
    joyNum: 1
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
    joyNum: 1
  - serializedVersion: 3
    m_Name: A
    descriptiveName: 
    descriptiveNegativeName: 
    negativeButton: 
    positiveButton: joystick 1 button 0
    altNegativeButton: 
    altPositiveButton: 
    gravity: 1000
    dead: 0.001
    sensitivity: 1000
    snap: 0
    invert: 0
    type: 0
    axis: 0
    joyNum: 1
  - serializedVersion: 3
    m_Name: B
    descriptiveName: 
    descriptiveNegativeName: 
    negativeButton: 
    positiveButton: joystick 1 button 1
    altNegativeButton: 
    altPositiveButton: 
    gravity: 1000
    dead: 0.001
    sensitivity: 1000
    snap: 0
    invert: 0
    type: 0
    axis: 0
    joyNum: 1
  - serializedVersion: 3
    m_Name: Y
    descriptiveName: 
    descriptiveNegativeName: 
    negativeButton: 
    positiveButton: joystick 1 button 3
    altNegativeButton: 
    altPositiveButton: 
    gravity: 1000
    dead: 0.001
    sensitivity: 1000
    snap: 0
    invert: 0
    type: 0
    axis: 0
    joyNum: 1
  - serializedVersion: 3
    m_Name: X
    descriptiveName: 
    descriptiveNegativeName: 
    negativeButton: 
    positiveButton: joystick 1 button 2
    altNegativeButton: 
    altPositiveButton: 
    gravity: 1000
    dead: 0.001
    sensitivity: 1000
    snap: 0
    invert: 0
    type: 0
    axis: 0
    joyNum: 1
  - serializedVersion: 3
    m_Name: LB
    descriptiveName: 
    descriptiveNegativeName: 
    negativeButton: 
    positiveButton: joystick 1 button 4
    altNegativeButton: 
    altPositiveButton: 
    gravity: 1000
    dead: 0.001
    sensitivity: 1000
    snap: 0
    invert: 0
    type: 0
    axis: 0
    joyNum: 1
  - serializedVersion: 3
    m_Name: RB
    descriptiveName: 
    descriptiveNegativeName: 
    negativeButton: 
    positiveButton: joystick 1 button 5
    altNegativeButton: 
    altPositiveButton: 
    gravity: 1000
    dead: 0.001
    sensitivity: 1000
    snap: 0
    invert: 0
    type: 0
    axis: 0
    joyNum: 1
  - serializedVersion: 3
    m_Name: LEFTSTICK
    descriptiveName: 
    descriptiveNegativeName: 
    negativeButton: 
    positiveButton: joystick 1 button 8
    altNegativeButton: 
    altPositiveButton: 
    gravity: 1000
    dead: 0.001
    sensitivity: 1000
    snap: 0
    invert: 0
    type: 0
    axis: 0
    joyNum: 1
  - serializedVersion: 3
    m_Name: RIGHTSTICK
    descriptiveName: 
    descriptiveNegativeName: 
    negativeButton: 
    positiveButton: joystick 1 button 9
    altNegativeButton: 
    altPositiveButton: 
    gravity: 1000
    dead: 0.001
    sensitivity: 1000
    snap: 0
    invert: 0
    type: 0
    axis: 0
    joyNum: 1
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
    joyNum: 1
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
    joyNum: 1
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
    joyNum: 1
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
    joyNum: 1
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
    joyNum: 1
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
    joyNum: 1
  - serializedVersion: 3
    m_Name: Debug Persistent
    descriptiveName: 
    descriptiveNegativeName: 
    negativeButton: 
    positiveButton: right shift
    altNegativeButton: 
    altPositiveButton: joystick 1 button 2
    gravity: 0
    dead: 0
    sensitivity: 0
    snap: 0
    invert: 0
    type: 0
    axis: 0
    joyNum: 1
  - serializedVersion: 3
    m_Name: Debug Multiplier
    descriptiveName: 
    descriptiveNegativeName: 
    negativeButton: 
    positiveButton: left shift
    altNegativeButton: 
    altPositiveButton: joystick 1 button 3
    gravity: 0
    dead: 0
    sensitivity: 0
    snap: 0
    invert: 0
    type: 0
    axis: 0
    joyNum: 1
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
    joyNum: 1
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
    joyNum: 1
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
    joyNum: 1
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
    joyNum: 1
  - serializedVersion: 3
    m_Name: Enable Debug Button 1
    descriptiveName: 
    descriptiveNegativeName: 
    negativeButton: 
    positiveButton: left ctrl
    altNegativeButton: 
    altPositiveButton: joystick 1 button 8
    gravity: 0
    dead: 0
    sensitivity: 0
    snap: 0
    invert: 0
    type: 0
    axis: 0
    joyNum: 1
  - serializedVersion: 3
    m_Name: Enable Debug Button 2
    descriptiveName: 
    descriptiveNegativeName: 
    negativeButton: 
    positiveButton: backspace
    altNegativeButton: 
    altPositiveButton: joystick 1 button 9
    gravity: 0
    dead: 0
    sensitivity: 0
    snap: 0
    invert: 0
    type: 0
    axis: 0
    joyNum: 1
  - serializedVersion: 3
    m_Name: Debug Reset
    descriptiveName: 
    descriptiveNegativeName: 
    negativeButton: 
    positiveButton: left alt
    altNegativeButton: 
    altPositiveButton: joystick 1 button 1
    gravity: 0
    dead: 0
    sensitivity: 0
    snap: 0
    invert: 0
    type: 0
    axis: 0
    joyNum: 1
  - serializedVersion: 3
    m_Name: Debug Next
    descriptiveName: 
    descriptiveNegativeName: 
    negativeButton: 
    positiveButton: page down
    altNegativeButton: 
    altPositiveButton: joystick 1 button 5
    gravity: 0
    dead: 0
    sensitivity: 0
    snap: 0
    invert: 0
    type: 0
    axis: 0
    joyNum: 1
  - serializedVersion: 3
    m_Name: Debug Previous
    descriptiveName: 
    descriptiveNegativeName: 
    negativeButton: 
    positiveButton: page up
    altNegativeButton: 
    altPositiveButton: joystick 1 button 4
    gravity: 0
    dead: 0
    sensitivity: 0
    snap: 0
    invert: 0
    type: 0
    axis: 0
    joyNum: 1
  - serializedVersion: 3
    m_Name: Debug Validate
    descriptiveName: 
    descriptiveNegativeName: 
    negativeButton: 
    positiveButton: return
    altNegativeButton: 
    altPositiveButton: joystick 1 button 0
    gravity: 0
    dead: 0
    sensitivity: 0
    snap: 0
    invert: 0
    type: 0
    axis: 0
    joyNum: 1
  - serializedVersion: 3
    m_Name: Menu
    descriptiveName: 
    descriptiveNegativeName: 
    negativeButton: 
    positiveButton: joystick 1 button 7
    altNegativeButton: 
    altPositiveButton: 
    gravity: 0
    dead: 0
    sensitivity: 0
    snap: 0
    invert: 0
    type: 0
    axis: 0
    joyNum: 1
";

        System.IO.File.WriteAllText(filepath, newText);
    }

    #endregion
}
