using GameTypes;
using UnityEngine;
using Random = UnityEngine.Random;

// ReSharper disable once CheckNamespace
public class TypedNode : MonoBehaviour
{
    [HideInInspector] public int microGamesNumber;
    public NodeBehaviour behaviour;
    [GameType(typeof(NodeDomainType))]
    public int type;

    [SerializeField] private GameObject hologramGO;
    [SerializeField] private SpriteRenderer sRenderer;
    [SerializeField] private SpriteRenderer logoSpriteRenderer;

    public void SetRandomType()
    {
        if (type != NodeDomainType.Random) return;
        
        int rdType = Random.Range(1, 7);
        type = rdType + 1;
        sRenderer.sprite = Resources.Load<SpriteListSO>("NodeSprites").nodeSprites[rdType];
        logoSpriteRenderer.sprite = Resources.Load<SpriteListSO>("NodeLogoSprites").nodeSprites[rdType - 1];
    }

    public void DisableNode()
    {
        sRenderer.sprite = Resources.Load<SpriteListSO>("NodeSprites").nodeSprites[0];
        hologramGO.SetActive(false);
        enabled = false;
    }

    private void Start()
    {
        SetRandomType();
    }
}
