using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ULB2_SFX : MonoBehaviour
{

    public void Sound(int index)
    {
        ULB2_Slider.instance.Play(index);
    }
}
