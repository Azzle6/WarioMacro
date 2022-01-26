using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public bool animationState = true;

    private void AnimationFinished()
    {
        animationState = false;
    }
}
