using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NAB2_FailureAnimation : MonoBehaviour
{
    private Animation failAnim;
    private void Start()
    {
        failAnim = gameObject.GetComponent<Animation>();
    }
    public void Animation()
    {
        failAnim.Play();
    }
}
