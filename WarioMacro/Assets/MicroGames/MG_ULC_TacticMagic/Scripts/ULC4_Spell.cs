using UnityEngine;

public class ULC4_Spell : MonoBehaviour
{
    private Animator playerAnimator, enemyAnimator;
    
    [SerializeField] private float speed;
    public Vector3 direction {private get; set;}
    public Object caster {private get; set;}

    private void Start()
    {
        playerAnimator = FindObjectOfType<ULC4_Player>().GetComponent<Animator>();
        enemyAnimator = FindObjectOfType<ULC4_Enemy>().GetComponent<Animator>();
    }


    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<ULC4_Player>() != null)
        {
            playerAnimator.SetTrigger("Hurt");
            ULC4_GameManager.instance.EndGame(false);
            ULC4_Pooler.instance.Depop("Spell", gameObject);
        }

        else if (other.GetComponent<ULC4_Enemy>() != null)
        {
            enemyAnimator.SetTrigger("Hurt");
            ULC4_Pooler.instance.Depop("Spell", gameObject);
        }
        
        else if (other.GetComponent<ULC4_Spell>() != null)
        {
            if (other.GetComponent<ULC4_Spell>().caster != caster)
            {
                ULC4_Pooler.instance.Depop("Spell", gameObject);
            }
        }
    }
}
