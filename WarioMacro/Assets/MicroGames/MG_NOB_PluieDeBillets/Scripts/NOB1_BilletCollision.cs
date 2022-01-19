using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NOB1_BilletCollision : MonoBehaviour
{
    int actualLives;

    public AudioClip misstake;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Destroy(gameObject);
        }
        else
        {
            actualLives = PlayerPrefs.GetInt("Lives");
            PlayerPrefs.SetInt("Lives", actualLives-1);
            AudioManager.PlaySound(misstake, 0.3f);
            Destroy(gameObject);
        }
    }
}
