using System.Collections.Generic;
using GameTypes;
using UnityEngine;

public class BehaviourNode : Node
{
    [HideInInspector] public int microGamesNumber;
    public NodeBehaviour behaviour;
    
    [SerializeField] private SpriteRenderer logoSpriteRenderer;
    private int primaryDomain = NodeDomainType.None;
    private int secondaryDomain = NodeDomainType.None;

    public void SetRandomDomain()
    {
        if (behaviour != NodeBehaviour.White) return;
        
        int rdType = Random.Range(1, 7);
        //type = rdType + 1;
        sRenderer.sprite = Resources.Load<SpriteListSO>("NodeSprites").nodeSprites[rdType];
        logoSpriteRenderer.sprite = Resources.Load<SpriteListSO>("NodeLogoSprites").nodeSprites[rdType - 1];
    }

    private void Start()
    {
        SetRandomDomain();
    }
}
