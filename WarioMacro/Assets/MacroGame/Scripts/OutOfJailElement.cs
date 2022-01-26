using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OutOfJailElement : MonoBehaviour
{
    public Character character;
    public TextMeshProUGUI priceTMP;
    public TextMeshProUGUI timeLeftTMP;
    public Image portrait;
    public Animator animator;
    EventSystemFocus test;
    
    private CharacterManager.Imprisoned imp;


    void Start()
    {
        portrait.sprite = character.cardSprite;
    }
    
    void Update()
    {
        SetSelected();
    }
    
    public void SetJail(CharacterManager.Imprisoned imprisoned)
    {
        priceTMP.text = imprisoned.price.ToString() +"$";
        timeLeftTMP.text = imprisoned.turnLeft.ToString();
        imp = imprisoned;
    }

    public void ReleaseCharacter()
    {
        CharacterManager.instance.FreeImprisoned(imp);
    }

    public void SetSelected()
    {
        if (EventSystem.current.currentSelectedGameObject == gameObject)
        {
            animator.SetBool("isSelected", true);
        }
        else
        {
            animator.SetBool("isSelected", false);
        }
    }
}
