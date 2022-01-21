using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NAC1_UIManager : MonoBehaviour
{
    [SerializeField] private Sprite[] signSprites = new Sprite[3];
    [SerializeField] private SpriteRenderer trueSigneSprite;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private GameObject endScreen, bodyGuard, defaite, victoire;
    [SerializeField] private TMP_Text resultText;
    public AudioClip audioClip , audioClip2;

   public bool win = false;
   public bool over = false;
    public void InitializeTrueSign(int trueSign)
    {
        trueSigneSprite.sprite = signSprites[trueSign];
    }

     void Awake()
    {
        GameManager.Register();
    }
    public void UpdateTimer(int nbTickLeft) 
    {
        timerText.text = nbTickLeft.ToString();
    }

    public void EnableEndScreen(bool result) 
    {
        if (result)
        {
             win = true;
           
            AudioManager.PlaySound(audioClip2, 1);
          //  resultText.text = "Good job !";
            victoire.SetActive(true);
            // bodyGuard.SetActive(true);
            
        }

        else
        {
            over = true;
            AudioManager.PlaySound(audioClip, 1);
           // resultText.text = "  You can't       enter !";
            bodyGuard.SetActive(true);
            defaite.SetActive(true);
        }

        endScreen.SetActive(true);

    }
}
