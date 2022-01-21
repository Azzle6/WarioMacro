using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NAB1_Animation : StateMachineBehaviour
{
    
    [SerializeField] private AudioClip victorySound;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        AudioManager.PlaySound(victorySound);
        NAB1_VictoryManager.instance.victory = true;
    }

}
