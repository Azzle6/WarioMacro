using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class NAA_UIManager_LAC : MonoBehaviour
{
    public NAA_MicroGameController_LAC MC_Controller;
    public uiStick rightStick, leftStick;

    public float stickAmp;
    public float stickSpeed = 1;

    public float stickMoveRefresh = 1.1f;
    public Animator uiAnimator;
    public TextMeshProUGUI endText;
    private void Start()
    {
        rightStick.stickData = MC_Controller.rightStick;
        leftStick.stickData = MC_Controller.leftStick;
    }

    private void Update()
    {

        // right stick
        if (Time.time - (rightStick.moveRefreshTime/Time.timeScale) > stickMoveRefresh)
        {
            rightStick.isMoving = rightStick.CheckStickMove();

            float arrowAngle = Vector2.Angle(rightStick.stickData.validStickDir, Vector2.right);
            rightStick.arrowDir.transform.eulerAngles = new Vector3(0, 0, arrowAngle);
        }
            

        if (rightStick.isMoving)
            rightStick.MovePad(stickAmp,stickSpeed);
        else
            rightStick.ResetPad(0.5f);

        //left stick
        if (Time.time - (leftStick.moveRefreshTime/Time.timeScale) > stickMoveRefresh)
        {
            leftStick.isMoving = leftStick.CheckStickMove();

            float arrowAngle = Vector2.Angle(leftStick.stickData.validStickDir, Vector2.right);
            leftStick.arrowDir.transform.eulerAngles = new Vector3(0, 0, arrowAngle);
        }
            

        if (leftStick.isMoving)
            leftStick.MovePad(stickAmp, stickSpeed);
        else
            leftStick.ResetPad(0.5f) ;

        if (!MC_Controller.isPlaying)
        {
            endText.text = (MC_Controller.result) ? "Victory !" : "Defeat...";
            uiAnimator.SetTrigger("End");
        }
    }

    [System.Serializable]
    public struct uiStick
    {
        [HideInInspector]
        public NAA_StickData_LAC stickData;
        public GameObject pad, arrowDir;
        float stopMoveTime;
        int lastMvtCount;

        [HideInInspector]
        public bool isMoving;
        [HideInInspector]
        public float moveRefreshTime;

        public void MovePad( float amp, float speed)
        {
            stopMoveTime = Time.time;
            float value = (Mathf.PingPong(Time.time * 100 * speed, 2*amp) - amp);
            pad.transform.position =  arrowDir.transform.position + (Vector3)stickData.validStickDir.normalized * -value ;
            
        }

        public void ResetPad(float duration = 1)
        {
            pad.transform.position = Vector3.Lerp(pad.transform.position, arrowDir.transform.position,(Time.time - (stopMoveTime/(Ticker.gameBPM / 120)))/duration);
        }

        public bool CheckStickMove()
        {
            moveRefreshTime = Time.time;
            if (lastMvtCount < stickData.mvtCount)
            {
                lastMvtCount = stickData.mvtCount;
                return true;
            }
            else
                return false;
        }

    }


}
