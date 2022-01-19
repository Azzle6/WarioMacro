using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NOB1_GeneralManager : MonoBehaviour, ITickable
{
    bool result = true;

    int finalLives;

    public GameObject keyword;

    public GameObject victory;
    public GameObject defeat;

    public AudioClip defeatSound;

    public GameObject firstMisstake;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Register();
        GameController.Init(this);

        if (GameController.difficulty == 1)
        {
            PlayerPrefs.SetInt("Lives", 3);
        }
        else if (GameController.difficulty == 2)
        {
            PlayerPrefs.SetInt("Lives", 2);
        } 
        else if (GameController.difficulty == 3)
        {
            PlayerPrefs.SetInt("Lives", 1);
            firstMisstake.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTick()
    {
        //Suppression Keyword
        if (GameController.currentTick == 1)
        {
            keyword.SetActive(false);
        }

        //Gestion état du minijeu
        if (GameController.currentTick == 5)
        {
            //Gestion résultat minijeu
            finalLives = PlayerPrefs.GetInt("Lives");
            if (finalLives > 0)
            {
                result = true;
            }
            else
            {
                result = false;
            }

            //Affichage résultat
            if (result == true)
            {
                victory.SetActive(true);
                //son audio clip
            }
            else
            {
                defeat.SetActive(true);
                AudioManager.PlaySound(defeatSound);
            }
            GameController.StopTimer();
        }
        else if (GameController.currentTick == 8)
        {
            GameController.FinishGame(result);
        }
    }

}
