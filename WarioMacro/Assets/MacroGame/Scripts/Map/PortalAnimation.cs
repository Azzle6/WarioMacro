using JetBrains.Annotations;
using UnityEngine;

// ReSharper disable once CheckNamespace
public class PortalAnimation : MonoBehaviour
{
    public bool animationState = true;

    [UsedImplicitly]
    private void PlayButtonSound()
    {
        AudioManager.MacroPlaySound("EmergencyButton");
    }
    
    [UsedImplicitly]
    private void PlayPortalSound()
    {
        AudioManager.MacroPlaySound("EmergencyPortal");
    }

    [UsedImplicitly]
    private void AnimationFinished()
    {
        animationState = false;
    }
}
