using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;

public class NAB1_PlayerMove : MonoBehaviour
{
    public string side;
    private int position;
    private int rotation;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private Animator movementAnimator;
    [SerializeField] private AudioClip moveSound;
    [SerializeField] private AudioClip defeatSound;
    [SerializeField] private TMP_Text victoryText;

    void Start()
    {
        
        if (Random.value >= 0.5)
        {
            side = "Left";
            playerAnimator.SetBool("Left", true);
        }
        else
        {
            side = "Right";
        }
        movementAnimator.SetBool(side, true);
        GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
    }

    private void Update()
    {
        if (NAB1_VictoryManager.instance.victory)
        {
            playerAnimator.SetBool("Walk", false);
            StartCoroutine(DisplayVictoryText());
        }
    }

    public void End()
    {
        Camera.main.GetComponent<Animator>().SetTrigger(side);
        AudioManager.StopSound(moveSound);
    }

    public void Spotted()
    {
        Camera.main.GetComponent<Animator>().SetBool("Seen", true);
        movementAnimator.SetFloat("MovementSpeed", 0f);
        AudioManager.StopSound(moveSound);
        AudioManager.PlaySound(defeatSound);
        playerAnimator.SetTrigger("Spotted");
    }

    public void Move()
    {
        AudioManager.PlaySound(moveSound);
        playerAnimator.SetBool("Walk", true);
        movementAnimator.SetBool("Moving", true);
    }

    private IEnumerator DisplayVictoryText()
    {
        yield return new WaitForSeconds(0.5f);

        victoryText.enabled = true;
    }
}
