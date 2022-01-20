using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NAA2_Surveillance_RandomSprite : MonoBehaviour
{

    [SerializeField]
    SpriteRenderer sr;
    public List<Sprite> sprites = new List<Sprite>();

    // Start is called before the first frame update
    void Start()
    {
        GameStart();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GameStart()
    {
        sr.sprite = sprites[Random.Range(0, sprites.Count)];
    }
}
