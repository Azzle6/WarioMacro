using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ULA2_ButtonPicker : MonoBehaviour
{
    public Sprite[] buttonSprites;

    public ULA2_ArmScript armScript;

    SpriteRenderer spriteRend;

    private void Start()
    {
        spriteRend = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        spriteRend.sprite = buttonSprites[armScript.randButton];
    }
}
