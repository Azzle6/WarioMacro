using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class NOC2_Ui_Animation : MonoBehaviour
{
    [SerializeField] private Image flash;

    public void Flash(float speed)
    {
        StartCoroutine(FlashCoroutine(speed));
    }

    IEnumerator FlashCoroutine(float speed)
    {
        flash.color = Color.white - new Color(0, 0, 0, 0.5f);
        flash.enabled = true;
        while (flash.color.a != 0)
        {
            yield return null;
            flash.color -= new Color(0, 0, 0, Time.unscaledDeltaTime * speed);
        }
        flash.enabled = false;
    }
}
