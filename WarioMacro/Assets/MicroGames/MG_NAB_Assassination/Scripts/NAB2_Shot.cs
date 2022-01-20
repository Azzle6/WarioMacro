using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NAB2_Shot : MonoBehaviour
{

    public Sprite shooting;

    public SpriteRenderer spriteRendering;
    public Image CrossHair;
    public bool targetAcquired;
    public bool win = false, shotFired = false;

    public NAB2_GameManager gameManager;

    public Vector3 screenSize;
    private Vector3 successVector;
    public GameObject successSprite;

    private float diffTime;
    public float timeToSpawnFun;

    public AudioClip shot, robot;

    void Update()
    {
        if (shotFired == false)
        {
            if (InputManager.GetKeyDown(ControllerKey.A) == true && targetAcquired == true)
            {
                spriteRendering.sprite = shooting;
                win = true;
                shotFired = true;
                AudioManager.PlaySound(shot);
                AudioManager.PlaySound(robot, 0.3f, 0.2f);
            }

            if (InputManager.GetKeyDown(ControllerKey.A) == true && targetAcquired != true)
            {
                shotFired = true;
                FailAnimation();
                AudioManager.PlaySound(shot);
                
            }
        }

        if(win == true)
        {
            if (Time.time - diffTime >= timeToSpawnFun)
            {
                diffTime = Time.time;
                SuccessAnimation();
            }
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        CrossHair.color = Color.green;
        targetAcquired = true;
    }

    private void OnTriggerExit(Collider collision)
    {
        CrossHair.color = Color.white;
        targetAcquired = false;
    }

    void SuccessAnimation()
    {
        successVector = new Vector3(Random.Range(-screenSize.x / 2, screenSize.x / 2), Random.Range(-screenSize.y / 2, screenSize.y / 2));
        Instantiate(successSprite, successVector, Quaternion.identity);
    }

    void FailAnimation()
    {

    }
}
