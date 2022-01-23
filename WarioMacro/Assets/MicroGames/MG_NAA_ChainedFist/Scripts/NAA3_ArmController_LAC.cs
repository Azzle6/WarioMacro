using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NAA3_ArmController_LAC : MonoBehaviour
{
    public NAA3_MicroGameController_LAC MC_Controller;
    GameObject armPlace;

    NAA3_StickData_LAC stickData;
    public Animator animator;

    float maxAngleRot = 30;
    public float rotSpeed = 10, resetRotSpeed = 10;

    public enum ArmType { RIGHT, LEFT}
    public ArmType armType;
    [Header("Visual")]
    public SpriteRenderer hand;
    public Sprite loseHand, winHand;

    [SerializeField] SpriteRenderer crystal;
    [SerializeField] Sprite brokenCrystal;

    float moveCheckTime, moveCheckFreq = 0.8f;
    int lastMvtCount;
    public bool moving;

    private void Start()
    {
        armPlace = transform.parent.gameObject;
        AssignStickData(armType);
        moveCheckTime = Time.time;
    }
    private void Update()
    {
        if(Time.time - moveCheckTime > moveCheckFreq)
        {
            moveCheckTime = Time.time;

            moving = CheckStickMove(stickData);
            animator.SetBool("Moving", moving && MC_Controller.isPlaying);

        }
        // rotation
        if (moving && MC_Controller.isPlaying)
            ArmRotation(rotSpeed);
        else
            ResetRotation(rotSpeed);

        if (!MC_Controller.isPlaying)
        {
            armPlace.transform.localEulerAngles = Vector3.zero;

            if (MC_Controller.result)
                crystal.sprite = brokenCrystal;

            hand.sprite = (MC_Controller.result) ? winHand : loseHand;
        }
            

    }
    void AssignStickData(ArmType armType)
    {
        if (armType == ArmType.RIGHT)
            stickData = MC_Controller.rightStick;

        if (armType == ArmType.LEFT)
            stickData = MC_Controller.leftStick;
    }

    bool CheckStickMove(NAA3_StickData_LAC stickData)
    {
        if (lastMvtCount < stickData.mvtCount)
        {
            lastMvtCount = stickData.mvtCount;
            return true;
        }
        else
            return false;
    }

    public void ArmRotation( float duration)
    {
        Vector2 rotVec = new Vector2(Mathf.Cos(transform.localEulerAngles.z * Mathf.Deg2Rad), Mathf.Sin(transform.localEulerAngles.z * Mathf.Deg2Rad));

        if (Vector2.Angle(Vector2.right, rotVec) < maxAngleRot) 
            transform.Rotate(new Vector3(0, 0, rotSpeed * Time.deltaTime * Mathf.Sign(transform.localScale.x)));
    }
    public void ResetRotation(float duration)
    {
        //Debug.Log("Rot : " + transform.eulerAngles.z);
        if (Mathf.Abs(transform.localEulerAngles.z % 360) > 5)
            transform.Rotate(new Vector3(0, 0, -resetRotSpeed * Time.deltaTime * Mathf.Sign(transform.localScale.x)));
        else
            transform.localEulerAngles = Vector3.zero;
    }
}
