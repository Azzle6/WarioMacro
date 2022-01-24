using GameTypes;
using UnityEngine;

public interface INode
{
    public void DisableNode();
}

public abstract class Node : MonoBehaviour, INode
{
    [SerializeField] private protected SpriteRenderer sRenderer;
    [SerializeField] private GameObject hologramGO;

    public void DisableNode()
    {
        sRenderer.sprite = Resources.Load<SpriteListSO>("NodeSprites").nodeSprites[0];
        hologramGO.SetActive(false);
        enabled = false;
    }
}
