using UnityEngine;

public class ULC_TM_Spell : MonoBehaviour
{
    private Animator playerAnimator, enemyAnimator;
    
    public Vector3 direction;
    public float speed;

    public Object caster;

    private void Start()
    {
        playerAnimator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        enemyAnimator = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Animator>();
    }


    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EndLinePlayer"))
        {
            playerAnimator.SetTrigger("Hurt");
            ULC_TM_GameManager.instance.LoseGame();
            ULC_TM_Pooler.instance.Depop("Spell", gameObject);
        }

        if (other.CompareTag("EndLineEnemy"))
        {
            enemyAnimator.SetTrigger("Hurt");
            ULC_TM_Pooler.instance.Depop("Spell", gameObject);
        }
        
        else if (other.CompareTag("Spell"))
        {
            if (other.GetComponent<ULC_TM_Spell>().caster != caster)
            {
                ULC_TM_Pooler.instance.Depop("Spell", gameObject);
            }
        }
    }
}
