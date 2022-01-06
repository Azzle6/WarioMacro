using UnityEngine;

public class ULC_TM_Spell : MonoBehaviour
{
    private Animator playerAnimator, enemyAnimator;
    
    [SerializeField] private float speed;
    public Vector3 direction {private get; set;}
    public Object caster {private get; set;}

    private void Start()
    {
        playerAnimator = FindObjectOfType<ULC_TM_Player>().GetComponent<Animator>();
        enemyAnimator = FindObjectOfType<ULC_TM_Enemy>().GetComponent<Animator>();
    }


    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<ULC_TM_Player>() != null)
        {
            playerAnimator.SetTrigger("Hurt");
            ULC_TM_GameManager.instance.LoseGame();
            ULC_TM_Pooler.instance.Depop("Spell", gameObject);
        }

        else if (other.GetComponent<ULC_TM_Enemy>() != null)
        {
            enemyAnimator.SetTrigger("Hurt");
            ULC_TM_Pooler.instance.Depop("Spell", gameObject);
        }
        
        else if (other.GetComponent<ULC_TM_Spell>() != null)
        {
            if (other.GetComponent<ULC_TM_Spell>().caster != caster)
            {
                ULC_TM_Pooler.instance.Depop("Spell", gameObject);
            }
        }
    }
}
