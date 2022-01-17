using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NAA1_Safe_SafeManager : MonoBehaviour, ITickable
{
    private AudioSource audioSource;

    public float BPM = 120f; //Tickrate = tick/second
    public float rotateSpeed = 1f; //Speed at which the safe quadrant turns
    public float numberDistance = 1f;
    public bool success = false;

    public int codeLength = 10; //Number of characters in the code
    public int difficulty = 0; //0 = easy ; 1 = medium ; 2 = hard

    public List<float> angles = new List<float>();
    public int angleAmount = 10;

    public int chosenNumber = 0;
    public int correctCode, inputCode, inputAmount;

    public GameObject numberIndicatorOriginal;
    public GameObject quadrant, failureImg, successImg;
    private Transform quadrantTransform;

    public TextMeshProUGUI correctCodeText, chosenCodeText;
    public Image scratches;

    public string codeString = "";

    void Start()
    {
        GameManager.Register(); //Mise en place du Input Manager, du Sound Manager et du Game Controller
        GameController.Init(this); //Permet a ce script d'utiliser le OnTick

        difficulty = GameController.difficulty;
        switch (difficulty)
        {
            case 1:
                codeLength = 2;
                break;
            case 2:
                codeLength = 3;
                break;
            case 3:
                codeLength = 3;
                scratches.gameObject.SetActive(true);
                break;
        }
        inputAmount = 0;
        
        correctCode = SetupCode();
        
        SetupAngles();
        
        quadrant = transform.GetChild(0).gameObject;
        quadrantTransform = quadrant.transform;
        quadrant.SetActive(true);
        
        codeString = TurnCodeToString(correctCode);
        correctCodeText.text = codeString;
        
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((InputManager.GetKeyDown(ControllerKey.A) 
             || InputManager.GetKeyDown(ControllerKey.B)
             || InputManager.GetKeyDown(ControllerKey.X)
             || InputManager.GetKeyDown(ControllerKey.Y)) && GameController.currentTick < 5)
        {
            SafeInput();
        }
        float tempAngle = quadrantTransform.eulerAngles.z;
        tempAngle -= rotateSpeed * BPM * Time.deltaTime;
        quadrantTransform.eulerAngles = new Vector3(0, 0, tempAngle);
    }
    
    public void OnTick()
    {
        if (GameController.currentTick == 5)
        {
            if (!success)
            {
                Debug.LogWarning("You failed !");
                failureImg.SetActive(true);
            }
            else
            {
                Debug.LogWarning("You won !");
                successImg.SetActive(true);
            }
        }

        if (GameController.currentTick == 8)
        {
            //Le jeu se décharge
            GameController.FinishGame(success);
        }
    }

    private int SetupCode()
    {
        int tempInt = 0;
        int savedInt=-1;
        for (int i = 0; i < codeLength; i++)
        {
            tempInt *= 10;
            int tempRand = UnityEngine.Random.Range(1, angleAmount);
            if (tempRand == savedInt)
            {
                if (tempRand >= angleAmount - 1)
                {
                    tempRand --;
                }
                else
                {
                    tempRand++;
                }
            }
            tempInt += tempRand;
            savedInt = tempRand;
        }
        return tempInt;
    }

    private string TurnCodeToString(int code)
    {
        string tempCodeString;
        int tens = 1;
        for (int i = 0; i < codeLength; i++)
        {
            tens *= 10;
        }
        if (correctCode < tens / 1000)
        {
            tempCodeString = "000" + code.ToString();
        } else if (correctCode < tens / 100)
        {
            tempCodeString = "00" + code.ToString();
        } else if (correctCode < tens / 10)
        {
            tempCodeString = "0" + code.ToString();
        } else if (correctCode < tens)
        {
            tempCodeString = "" + code.ToString();
        }
        else
        {
            tempCodeString = code.ToString();
        }

        return tempCodeString;
    }

    private void SetupAngles()
    {
        angles = new List<float>();
        for (int i = 0; i < angleAmount; i++)
        {
            angles.Add((360/angleAmount*i));
            Vector3 tempRotation = Quaternion.Euler(0, 0,- 360 / angleAmount * i) * Vector3.up;
            tempRotation *= numberDistance;
            GameObject tempObj = Instantiate(numberIndicatorOriginal, transform.position + tempRotation, Quaternion.identity, transform);
            tempObj.GetComponentInChildren<Text>().text = i.ToString();
            tempObj.name = "";
            tempObj.GetComponent<CircleCollider2D>().radius = ((2 * numberDistance) * Mathf.Sin(Mathf.PI / angleAmount))/2;
            for (int j = 0; j < i; j++)
            {
                tempObj.name += "i";
            }
        }
    }

    private void SetupQuadrant()
    {
        quadrant.GetComponent<Collider2D>().offset = new Vector2(0, numberDistance);
    }

    public void SafeInput()
    {
        inputAmount++;
        audioSource.Play();
        rotateSpeed *= -1;
        inputCode *= 10;
        inputCode += chosenNumber;
        chosenCodeText.text = TurnCodeToString(inputCode);
        if (inputCode == correctCode)
        {
            Debug.LogWarning("You win !");
            StartCoroutine(BeepCoroutine());
            transform.parent.parent.GetComponent<Animation>().Play();
            success = true;
            successImg.SetActive(true);
        } else if (inputAmount == codeLength)
        {
            inputCode = 0;
            inputAmount = 0;
            chosenCodeText.text = "XXX";
            
        }
    }

    private IEnumerator BeepCoroutine()
    {
        yield return new WaitForSeconds(0.3f);
        audioSource.Play();
    }
}
