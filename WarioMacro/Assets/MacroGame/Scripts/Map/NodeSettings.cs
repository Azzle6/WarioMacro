using System;
using GameTypes;
using UnityEngine;
using Random = UnityEngine.Random;

// ReSharper disable once CheckNamespace
public class NodeSettings : MonoBehaviour
{
    [HideInInspector] public int microGamesNumber;
    [GameType(typeof(NodeType))]
    public int type;
    
    [SerializeField] private SpriteRenderer sRenderer;

    public void SetRandomType()
    {
        if (type != NodeType.Random) return;
        
        int rdType = Random.Range(0, 6);
        type = rdType + 2;
        sRenderer.sprite = Resources.Load<NodeRandomSO>("NodeSprites").nodeSprites[rdType];
    }

    private void Start()
    {
        SetRandomType();
    }
}
