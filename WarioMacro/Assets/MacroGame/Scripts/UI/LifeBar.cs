using System;
using UnityEngine;
using UnityEngine.UI;

public class LifeBar : MonoBehaviour
{
    [SerializeField] private Transform lifeBarGO;
    [Range(0, 4)]
    [SerializeField] private int life;

    [SerializeField] private Image[] portraits;
    public Character charatest;

    private void Start()
    {
        life = 0;
    }

    /*private void Update()
    {
        if (InputManager.GetKeyDown(ControllerKey.B)) Imprison();
        if (InputManager.GetKeyDown(ControllerKey.X)) RecruitCharacter(charatest);
    }*/

    public void RecruitCharacter(Character chara)
    {
        if (life == 4) return;
        life++;
        portraits[life-1].sprite = chara.lifebarSprite;
        portraits[life - 1].enabled = true;


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
