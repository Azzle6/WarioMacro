using System.Collections;
using UnityEngine;

public class ULC4_Enemy : MonoBehaviour
{
    private Animator animatorEnemy;
    
    [SerializeField] private float timeBetweenSpells, chanceOfDoubleFire;
    private bool canFire;
    
    [SerializeField] private AudioClip[] spellSounds;

    private void Start()
    {
        canFire = true;
        animatorEnemy = GetComponent<Animator>();
        StartCoroutine(FireCoroutine());
    }

    private IEnumerator FireCoroutine()
    {
        yield return new WaitForSeconds(1f);
        
        while (ULC4_GameManager.instance.inGame && canFire)
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
        if (rdm == 1) ULC4_SpellManager.instance.TopSpell(this);
        else if (rdm == 2) ULC4_SpellManager.instance.MidSpell(this);
        else ULC4_SpellManager.instance.BotSpell(this);
        AudioManager.PlaySound(spellSounds[Random.Range(0,3)],0.2f);
    }

    private void DoubleFire()
    {
        ULC4_SpellManager.instance.MidSpell(this);
        
        int rdm = Random.Range(1, 3);
        if (rdm == 1) ULC4_SpellManager.instance.TopSpell(this);
        else ULC4_SpellManager.instance.BotSpell(this);
        AudioManager.PlaySound(spellSounds[Random.Range(0,3)],0.2f);
    }
    
    public void setChanceOfDF(float chance) {chanceOfDoubleFire = chance;}

    public void stopFire() {canFire = false;}
}
