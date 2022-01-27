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
        
        portraits[life].sprite = chara.portraitSprite;
        portraits[life].enabled = true;
        keyChains[life].sprite = Resources.Load<SpriteListSO>("KeyChainSprites")
            .nodeSprites[chara.characterType - SpecialistType.Brute];
        life++;
    }

    public void Imprison(Character chara)
    {
        AudioManager.MacroPlaySound("CharacterLose", 0);
        for (int i = 0; i < portraits.Length; i++)
        {
            if (chara.portraitSprite != portraits[i].sprite) continue;
            
            lifeBarGO.GetChild(i).GetComponent<Animator>().SetBool("Anim", true); //à remplacer par le trigger de l'animation
            break;
        }
        
        
        life--;
    }

    public int GetLife() => life;
}
