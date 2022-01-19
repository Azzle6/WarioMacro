using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NAA2_Surveillance_ChangeFloor : MonoBehaviour
{
    public List<Sprite> sprites = new List<Sprite>();
    [SerializeField]
    SpriteRenderer sr;
    [SerializeField]
    Animator animator;
    
    public float bpm;
    float elapsedTime;
    float bps;

    Sprite lastSprite;
    int randomizer;
    [SerializeField]
    AnimationClip clip;
    AnimationEvent evt = new AnimationEvent();

    // Start is called before the first frame update
    void Start()
    {

        sr.sprite = sprites[Random.Range(0, sprites.Count)];

       /* bps = bpm / 60;
        elapsedTime = 1/(4*bps);
        animator.speed = bps;
        evt.functionName = "ChangeSprite";
        
        ChangeSprite();*/
    }

    // Update is called once per frame
    void Update()
    {
       
        
    }

    void ChangeSprite()
    {

        randomizer = Random.Range(0, sprites.Count);
        if(lastSprite == sprites[randomizer])
        {
            if(randomizer+1 >= sprites.Count)
            {
                randomizer--;
            }
            else
            {
                randomizer++;
            }
        }

        lastSprite = sprites[randomizer];
        sr.sprite = lastSprite;
        clip.AddEvent(evt);
        animator.Play(clip.name, 0);
    }
}
