using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
    public GameObject[] arrowPrefabs = new GameObject[4];

    [FormerlySerializedAs("playerAnimation")] [SerializeField] private PortalAnimation portalAnimation;
    [SerializeField] private Transform puppetMainCharacter;
    [SerializeField] private Animator animator;
    [SerializeField] private float moveSpeed = 1f;
    private static readonly int move = Animator.StringToHash("Move");
    private static readonly int idle = Animator.StringToHash("Idle");
    private static readonly int portalEnter = Animator.StringToHash("PortalEnter");
    private static readonly int portalExit = Animator.StringToHash("PortalExit");

    public void TeleportPlayer(Vector3 pos)
    {
        transform.position = pos;
    }
    
    public IEnumerator MoveToPosition(List<Transform> positions)
    {
        StartMove();
        foreach (Vector3 position in positions.Select(t => t.position))
        {
            FlipSpriteX(position.x < transform.position.x - 0.1f);
            var tween = transform.DOMove(position, moveSpeed).SetSpeedBased().SetEase(Ease.Linear);

            while (tween.IsPlaying()) yield return null;
        }

        StopMove();
    }

    public IEnumerator EnterPortal()
    {
        animator.SetTrigger(portalEnter);
        while (portalAnimation.animationState)
            yield return null;
        portalAnimation.animationState = true;
    }
    
    public IEnumerator ExitPortal()
    {
        animator.SetTrigger(portalExit);
        //while (playerAnimation.animationState) yield return null;
        portalAnimation.animationState = true;
        yield return null;
        //animator.SetTrigger(idle);
    }
    

    private void StartMove()
    {
        animator.SetTrigger(move);
        AudioManager.MacroPlaySound("NodeSelect", 0);
    }

    private void StopMove()
    {
        animator.SetTrigger(idle);
        FlipSpriteX(false);
    }

    private void FlipSpriteX(bool flip)
    {
        for (int i = 0; i < puppetMainCharacter.childCount; i++)
        {
            var renderer = puppetMainCharacter.GetChild(i).GetComponent<SpriteRenderer>();

            if (renderer != null)
            {
                renderer.flipX = flip;
            }
        }
    }
    
    private void Awake()
    {
        arrowPrefabs.ToList().ForEach(go => go.SetActive(false));
    }
}
