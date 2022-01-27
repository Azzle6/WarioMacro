using System;
using UnityEngine;

public class ActiveNodeInput : MonoBehaviour
{
    [SerializeField] private Map map;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private NodeVisual node;

    private void Update()
    {
        spriteRenderer.enabled = map.currentNode == node;
    }
}
