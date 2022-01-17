using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NAA3_StickData_LAC : MonoBehaviour
{
    [Header("Stick PARAM")]
    public ControllerAxis xAxis, yAxis;
    Vector2 stickDir;
    float stickLength;

    [Header("Check PARAM")]
    [Range(0, 1)]
    public float dirThreshold;
    public Vector2 validStickDir;
    [HideInInspector]
    public Vector2 stickValue;
    [Range(0,1)]
    public float checkLength, resetLength;
    public bool stickReset;

    [Header("Bpm PARAM")]
    public int mvtCount = 0;
    public float bpm; // approx : 200 mvt per minute
    float startTime;

    private void Start()
    {
        GameManager.Register(); //Mise en place du Input Manager, du Sound Manager et du Game Controller
        startTime = Time.unscaledTime;
    }
    private void Update()
    {
        // Stick Param
        stickValue = new Vector2(InputManager.GetAxis(xAxis), InputManager.GetAxis(yAxis));
        stickDir =  stickValue.normalized;
        stickLength = stickValue.magnitude;

        if (stickLength <= checkLength)
            stickReset = true;
        //dotStick = Mathf.Abs(Vector2.Dot(stickDir, validStickDir.normalized));
        if (stickLength > checkLength && stickReset && Mathf.Abs(Vector2.Dot(stickDir,validStickDir.normalized)) > dirThreshold)
        {
            stickReset = false;
            mvtCount++;
        }

        // Bpm count
        bpm = (mvtCount * 0.5f / (Time.unscaledTime - (startTime)))* 60;
    }
}
