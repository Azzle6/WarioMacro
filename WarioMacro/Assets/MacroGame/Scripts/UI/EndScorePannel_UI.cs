using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class EndScorePannel_UI : MonoBehaviour
{

    [Header("Score")]
    [SerializeField] private GameObject scorePannel;
    [SerializeField] private PlayableDirector scoreDirector;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            scorePannel.SetActive(true);
            scoreDirector.Play();
        }
    }

}
