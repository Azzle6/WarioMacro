using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class NAA3_UIManager_LAC : MonoBehaviour
{
    public NAA3_MicroGameController_LAC MC_Controller;
    [SerializeField] Image electricBar;
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
        // Electric Bar
        electricBar.fillAmount = (MC_Controller.currentBpm()/ (MC_Controller.minimumBpmRequire/0.75f));
        // right stick 
        if (Time.unscaledTime - (rightStick.moveRefreshTime) > stickMoveRefresh)
        {
            rightStick.isMoving = rightStick.CheckStickMove();

            float arrowAngle = Vector2.Angle(rightStick.stickData.validStickDir, Vector2.right);
            rightStick.arrowDir.transform.eulerAngles = new Vector3(0, 0, arrowAngle);
        }
            

        if (rightStick.isMoving)
            rightStick.MovePad(stickAmp,stickSpeed);
        else
            rightStick.ResetPad();

        //left stick
        if (Time.unscaledTime - (leftStick.moveRefreshTime) > stickMoveRefresh)
        {
            leftStick.isMoving = leftStick.CheckStickMove();

            float arrowAngle = Vector2.Angle(leftStick.stickData.validStickDir, Vector2.right);
            leftStick.arrowDir.transform.eulerAngles = new Vector3(0, 0, arrowAngle);
        }
            

        if (leftStick.isMoving)
            leftStick.MovePad(stickAmp, stickSpeed);
        else
            leftStick.ResetPad();

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
        public NAA3_StickData_LAC stickData;
        public GameObject stick,pad, arrowDir;
        float stopMoveTime;
        int lastMvtCount;

        [HideInInspector]
        public bool isMoving;
        [HideInInspector]
        public float moveRefreshTime;

        public void MovePad( float amp, float speed)
        {
            stopMoveTime = Time.unscaledTime;
            float value = (Mathf.PingPong(Time.unscaledTime * 100 * speed, 2*amp) - amp);
            pad.transform.position =  arrowDir.transform.position + (Vector3)stickData.validStickDir.normalized * -value ;
            
        }

        public void ResetPad(float duration = 1)
        {
            pad.transform.position = Vector3.Lerp(pad.transform.position, arrowDir.transform.position,(Time.unscaledTime - stopMoveTime)/duration);
        }


        public bool CheckStickMove()
        {
            moveRefreshTime = Time.unscaledTime;
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
