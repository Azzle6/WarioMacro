using UnityEngine;

public class ULB_Unlock_PlayerCollision : MonoBehaviour
{
    [SerializeField] private AudioClip metalsound;
    private void OnCollisionEnter(Collision other)
    {
        AudioManager.PlaySound(metalsound, 0.3f);
    }
}
