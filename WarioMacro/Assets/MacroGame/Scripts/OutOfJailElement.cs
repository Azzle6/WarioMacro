using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
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
    private static readonly int selection = Animator.StringToHash("Selection");


    void Start()
    {
        portrait.sprite = character.portraitSprite;
    }
    
    void Update()
    {
        SetSelected();
    }
    
    public void SetJail(CharacterManager.Imprisoned imprisoned)
    {
        priceTMP.text = imprisoned.price +"$";
        timeLeftTMP.text = imprisoned.turnLeft.ToString();
        imp = imprisoned;
    }

    public void ReleaseCharacter()
    {
        if (CharacterManager.instance.FreeImprisoned(imp))
        {
            animator.SetTrigger(selection);
        }
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

    [UsedImplicitly]
    private void DisableElement()
    {
        gameObject.SetActive(false);
    }
}
