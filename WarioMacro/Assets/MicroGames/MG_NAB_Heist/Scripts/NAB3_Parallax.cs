using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NAB3_Parallax : MonoBehaviour
{
    public float additionalScrollSpeed;

    public GameObject[] backgrounds;

    public float[] scrollSpeed;

    void FixedUpdate()
    {
        for (int background = 0; background < backgrounds.Length; background++)
        {
            Renderer rend = backgrounds[background].GetComponent<Renderer>();

            float offset = Time.time * (scrollSpeed[background] + additionalScrollSpeed);

            rend.material.SetTextureOffset("_MainTex", new Vector2(offset, 0));
        }
    }

}
