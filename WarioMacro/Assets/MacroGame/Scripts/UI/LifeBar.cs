using GameTypes;
using UnityEngine;
using UnityEngine.UI;

public class LifeBar : MonoBehaviour
{
    [SerializeField] private Transform lifeBarGO;
    [SerializeField] private Image[] portraits;
    [SerializeField] private Image[] keyChains;
    private int life;

    private void Start()
    {
        life = 0;
    }

    public void RecruitCharacter(Character chara)
    {
        if (life == 4) return;
        
        portraits[life].sprite = chara.lifebarSprite;
        portraits[life].enabled = true;
        keyChains[life].sprite = Resources.Load<SpriteListSO>("KeyChainSprites")
            .nodeSprites[chara.characterType - CharacterType.Scoundrel];
        life++;
    }

    public void Imprison()
    {
        AudioManager.MacroPlaySound("CharacterLose", 0);
        lifeBarGO.GetChild(life-1).GetComponent<Animator>().SetBool("Anim", true); //à remplacer par le trigger de l'animation
        lifeBarGO.GetChild(life-1).GetChild(0).gameObject.SetActive(true);
        
        life--;
    }

    public int GetLife() => life;
}
