using UnityEngine;
using Random = UnityEngine.Random;

// ReSharper disable once CheckNamespace
public class NodeSettings : MonoBehaviour
{
    public int microGamesNumber;
    public Type type;
    
    [SerializeField] private SpriteRenderer sRenderer;

    private void Start()
    {
        if (type != Type.Random) return;
        
        int rdType = Random.Range(0, 6);
        type = (Type) (rdType + 2);
        sRenderer.sprite = Resources.Load<NodeRandomSO>("NodeSprites").nodeSprites[rdType];
    }

    // TODO : synchronise with player types
    public enum Type 
    {
        None,
        Random,
        Brute,
        Alchemist,
        Expert,
        Ghost,
        Acrobat,
        Technomancer
    }
}
