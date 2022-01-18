using UnityEngine;
using UnityEngine.UI;

public class LifeBar : MonoBehaviour
{
    [SerializeField] private Transform lifeBarGO;
    [Range(1, 4)]
    [SerializeField] private int life;
    
    /*private void Start()
    {
        for (int i = 0; i < life; i++)
        {
            lifeBarGO.GetChild(i).gameObject.SetActive(true);
        }
    }*/

    public void Damage()
    {
        life--;
        
        lifeBarGO.GetChild(life).gameObject.SetActive(false);
    }

    public void RecruitCharacter(Character chara)
    {
        GetLife();
        lifeBarGO.GetChild(life - 1).GetComponent<Image>().sprite = chara.lifebarSprite;
        lifeBarGO.GetChild(life-1).gameObject.SetActive(true);
        
    }

    public void Imprison()
    {
        lifeBarGO.GetChild(life-1).gameObject.SetActive(false); //à remplacer par le trigger de l'animation
        life--;
    }

    public int GetLife() => life;
}
