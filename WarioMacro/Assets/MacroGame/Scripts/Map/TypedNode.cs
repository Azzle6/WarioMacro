using GameTypes;
using UnityEngine;
using Random = UnityEngine.Random;

// ReSharper disable once CheckNamespace
public class TypedNode : MonoBehaviour
{
    [HideInInspector] public int microGamesNumber;
    [GameType(typeof(NodeType))]
    public int type;
    
    [SerializeField] private SpriteRenderer sRenderer;
    [SerializeField] private SpriteRenderer logoSpriteRenderer;

    public void SetRandomType()
    {
        if (type != NodeType.Random) return;
        
        int rdType = Random.Range(0, 6);
        type = rdType + 2;
        sRenderer.sprite = Resources.Load<SpriteListSO>("NodeSprites").nodeSprites[rdType];
        logoSpriteRenderer.sprite = Resources.Load<SpriteListSO>("NodeLogoSprites").nodeSprites[rdType];
    }

    private void Start()
    {
        SetRandomType();
    }
}
