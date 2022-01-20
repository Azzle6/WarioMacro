using UnityEngine;

public class NAB3_NMI : MonoBehaviour
{
    public NAB3_NMIGenerator enemyGenerator;
    public AudioClip audioClip;

    void Awake()
    {
        AudioManager.Register();
    }
    
    void Update()
    {
        transform.Translate(Vector2.left * enemyGenerator.currentSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("nextLine"))
        {
            enemyGenerator.GenerateNextDogWithGap();
            PlaySound();
        }

        if (collision.gameObject.CompareTag("finishLine"))
        {
            StopSound(); 
            Destroy(this.gameObject);

        }
    }

    private void PlaySound()
    {
        AudioManager.PlaySound(audioClip);
    }

    private void StopSound()
    {
        AudioManager.StopSound(audioClip);
    }

}
