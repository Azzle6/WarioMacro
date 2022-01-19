using UnityEngine;

public class ULB1_PlayerCollision : MonoBehaviour
{
    [SerializeField] private AudioClip metalsound;
    private void OnCollisionEnter(Collision other)
    {
        AudioManager.PlaySound(metalsound, 0.3f);
    }
}
