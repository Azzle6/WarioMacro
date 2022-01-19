using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NOB1_BombCollision : MonoBehaviour
{
    int actualLives;

    public AudioClip misstake;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            actualLives = PlayerPrefs.GetInt("Lives");
            PlayerPrefs.SetInt("Lives", actualLives - 1);
            AudioManager.PlaySound(misstake, 0.3f);
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}