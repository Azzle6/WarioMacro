using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ULC_TM_Player : MonoBehaviour
{
    public Animator animator;
    
    [SerializeField] private Sprite[] bulletChargerSprites;
    [SerializeField] private Image myCharger;

    [SerializeField] private float reloadTime;
    private int nbBullets = 7;

    void Update()
    {
        myCharger.sprite = bulletChargerSprites[nbBullets];

        if (nbBullets != 7 && !isReloadCRRunning) StartCoroutine(ReloadBulletCoroutine());
        
        if (InputManager.GetKeyDown(ControllerKey.Y))
        {
            animator.SetTrigger("Shoot");
            TopSpell();
        }
        
        if (InputManager.GetKeyDown(ControllerKey.B))
        {
            animator.SetTrigger("Shoot");
            MidSpell();
        }
        if (InputManager.GetKeyDown(ControllerKey.A))
        {
            animator.SetTrigger("Shoot");
            BotSpell();
        }
    }

    private bool isReloadCRRunning;
    private IEnumerator ReloadBulletCoroutine()
    {
        isReloadCRRunning = true;
        
        while (nbBullets != 7)
        {
            yield return new WaitForSeconds(reloadTime);
            nbBullets++;
        }

        isReloadCRRunning = false;
    }
    
    public void TopSpell()
    {
        Debug.Log(nbBullets);
        if (nbBullets > 0)
        {
            ULC_TM_SpellManager.instance.TopSpell(this);
            nbBullets--;
        }
    }
    
    public void MidSpell()
    {
        if (nbBullets > 0)
        {
            ULC_TM_SpellManager.instance.MidSpell(this);
            nbBullets--;
        }
    }
    
    public void BotSpell()
    {
        if (nbBullets > 0)
        {
            ULC_TM_SpellManager.instance.BotSpell(this);
            nbBullets--;
        }
    }
}
