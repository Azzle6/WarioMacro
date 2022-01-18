using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ULA1_AnimationManager : MonoBehaviour
{
    [SerializeField] private Animation CameraAnimation;
    [SerializeField] private Animation PlayerAnimation;
    [SerializeField] private Animation EnnemiAnimation;
    // Start is called before the first frame update
    void Start()
    {
        PlayerAnimation.Play();
        StartCoroutine(CoroutineAnimationPlayDelayed(0.25f, EnnemiAnimation));
        StartCoroutine(CoroutineAnimationPlayDelayed(0.8f, CameraAnimation));
    }
    
    IEnumerator CoroutineAnimationPlayDelayed(float i, Animation anim)
    {
        yield return new WaitForSeconds(i);
        anim.Play();
    }
}
