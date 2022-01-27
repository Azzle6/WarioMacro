using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ULC1_Phatass_DificultyManager : MonoBehaviour
{
    //[SerializeField] private GameObject keyRightPocket, keyLeftPocket, emptyRightPocket, emptyLeftPocket;
    [SerializeField]
    private SpriteRenderer keyLeftPocket, tagKeyLeftPocket, keyRightPocket, emptyLeftPocket, emptyRightPocket;
    [SerializeField] private BoxCollider2D leftKeyCollider, rightKeyCollider;
    private Animator phatassAnimator;

    void Start()
    {
        phatassAnimator = GetComponent<Animator>();
        if (GameController. difficulty == 1)
        {
            phatassAnimator.SetTrigger("IsEasy");
            emptyLeftPocket.enabled = false;
            keyRightPocket.enabled = false;
            rightKeyCollider.enabled = false;
        }
        else if (GameController. difficulty == 2)
        {
            phatassAnimator.SetTrigger("IsNormal");
            emptyRightPocket.enabled = false;
            keyLeftPocket.enabled = false;
            tagKeyLeftPocket.enabled = false;
            leftKeyCollider.enabled = false;
        }
        else if (GameController. difficulty == 3)
        {
            phatassAnimator.SetTrigger("IsHard");
            emptyLeftPocket.enabled = false;
            keyRightPocket.enabled = false;
            rightKeyCollider.enabled = false;
        }
    }
}
