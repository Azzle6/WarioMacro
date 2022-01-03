using System.Collections;
using UnityEngine;

public class ULC_TM_Enemy : MonoBehaviour
{
    public Animator animatorEnemy;
    
    public float timeBetweenSpells;
    public float chanceOfDoubleFire;

    private void Start()
    {
        StartCoroutine(FireCoroutine());
    }

    private IEnumerator FireCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        
        while (ULC_TM_GameManager.instance.isRunning)
        {
            if (Random.Range(0f,1f) < chanceOfDoubleFire) DoubleFire();
            else Fire();

            yield return new WaitForSeconds(timeBetweenSpells);
        }
    }

    private void Fire()
    {
        animatorEnemy.SetTrigger("Shoot");
        int rdm = Random.Range(1, 4);
        if (rdm == 1) ULC_TM_SpellManager.instance.TopSpell(this);
        else if (rdm == 2) ULC_TM_SpellManager.instance.MidSpell(this);
        else ULC_TM_SpellManager.instance.BotSpell(this);
    }

    private void DoubleFire()
    {
        ULC_TM_SpellManager.instance.MidSpell(this);
        
        int rdm = Random.Range(1, 3);
        if (rdm == 1) ULC_TM_SpellManager.instance.TopSpell(this);
        else ULC_TM_SpellManager.instance.BotSpell(this);
    }
}
