using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NOB1_MisstakesManager : MonoBehaviour
{
    int actualLives;

    //public Text lives;
    public GameObject firstMisstake;
    public GameObject secondMisstake;
    public GameObject thirdMisstake;

    public AudioClip bombExplosion;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        actualLives = PlayerPrefs.GetInt("Lives");
        //lives.text = "Lives : " + actualLives;
        if (actualLives == 2)
        {
            firstMisstake.SetActive(true);
        } 
        else if (actualLives == 1)
        {
            secondMisstake.SetActive(true);
        }
        else if (actualLives == 0)
        {
            thirdMisstake.SetActive(true);
            AudioManager.PlaySound(bombExplosion, 0.2f);
        }
    }
}
