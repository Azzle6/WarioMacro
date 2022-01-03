using UnityEngine;

public class ULC_TM_Spell : MonoBehaviour
{
    public Animator exploseAnimator;
    public Animator enemyAnimator;
    public Vector3 direction;
    public float speed;

    public Object caster;

    private void Start()
    {
        exploseAnimator = GameObject.FindGameObjectWithTag("Spell").GetComponent<Animator>();
        enemyAnimator = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Animator>();
    }


    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EndLine"))
        {
            if (direction == Vector3.left) ULC_TM_GameManager.instance.LoseGame();
            ULC_TM_Pooler.instance.Depop("Spell", gameObject);
        }

        if (other.CompareTag("EndLineEnemy"))
        {
            enemyAnimator.SetTrigger("Hurt");
            if (direction == Vector3.left) ULC_TM_GameManager.instance.LoseGame();
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
