using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ULC4_Player : MonoBehaviour
{
    private Animator animatorPlayer;
    
    [SerializeField] private Sprite[] bulletChargerSprites;
    [SerializeField] private Image myCharger;

    [SerializeField] private float reloadTime;
    private int nbBullets = 6;

    void Start() {
        animatorPlayer = GetComponent<Animator>();
    }

    void Update()
    {
        if (!ULC4_GameManager.instance.inGame) return;
        
        myCharger.sprite = bulletChargerSprites[nbBullets];

        if (nbBullets != 7 && !isReloadCRRunning) StartCoroutine(ReloadBulletCoroutine());
        
        if (InputManager.GetKeyDown(ControllerKey.Y))
        {
            animatorPlayer.SetTrigger("Shoot");
            TopSpell();
        }
        
        if (InputManager.GetKeyDown(ControllerKey.B))
        {
            animatorPlayer.SetTrigger("Shoot");
            MidSpell();
        }
        if (InputManager.GetKeyDown(ControllerKey.A))
        {
            animatorPlayer.SetTrigger("Shoot");
            BotSpell();
        }
    }

    private bool isReloadCRRunning;
    private IEnumerator ReloadBulletCoroutine()
    {
        isReloadCRRunning = true;
        
        while (nbBullets != 6)
        {
            yield return new WaitForSeconds(reloadTime);
            nbBullets++;
        }

        isReloadCRRunning = false;
    }
    
    public void TopSpell()
    {
        if (nbBullets > 0)
        {
            ULC4_SpellManager.instance.TopSpell(this);
            nbBullets--;
        }
    }
    
    public void MidSpell()
    {
        if (nbBullets > 0)
        {
            ULC4_SpellManager.instance.MidSpell(this);
            nbBullets--;
        }
    }
    
    public void BotSpell()
    {
        if (nbBullets > 0)
        {
            ULC4_SpellManager.instance.BotSpell(this);
            nbBullets--;
        }
    }

    public void setReloadTime(float reloadTime) {this.reloadTime = reloadTime;}
}
