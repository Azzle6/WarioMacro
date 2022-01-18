using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersoVie_UI : MonoBehaviour
{

    [SerializeField] private Animator animator;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger("Anim");
        }
    }
}
