using UnityEngine;

public class NAB3_Explosion : MonoBehaviour
{
    public GameObject explosion;
    public GameObject dog;
    public GameObject question;
    public AudioClip audioClip1;
    public AudioClip audioClip2;


    private void Awake()
    {
        AudioManager.Register();
    }

    public void _Explosion()
    {
        explosion.SetActive(true);
        dog.SetActive(false);
        AudioManager.PlaySound(audioClip1);
        AudioManager.StopSound(audioClip2);
    }

    public void Question()
    {
        question.SetActive(true);
        AudioManager.PlaySound(audioClip2);
    }
}
