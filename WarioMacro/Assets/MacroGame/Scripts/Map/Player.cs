using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject[] arrowPrefabs = new GameObject[4];

    [SerializeField] private Transform puppetMainCharacter;
    [SerializeField] private Animator animator;
    [SerializeField] private float moveSpeed = 1f;
    private static readonly int move = Animator.StringToHash("Move");
    private static readonly int idle = Animator.StringToHash("Idle");

    public void TeleportPlayer(Vector3 pos)
    {
        transform.position = pos;
    }
    
    public IEnumerator MoveToPosition(List<Transform> positions)
    {
        StartMove();
        foreach (Vector3 position in positions.Select(t => t.position))
        {
            FlipSpriteX(position.x < transform.position.x);
            var tween = transform.DOMove(position, moveSpeed).SetSpeedBased().SetEase(Ease.Linear);

            while (tween.IsPlaying()) yield return null;
        }

        StopMove();
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
            puppetMainCharacter.GetChild(i).GetComponent<SpriteRenderer>().flipX = flip;
        }
    }
    
    private void Awake()
    {
        arrowPrefabs.ToList().ForEach(go => go.SetActive(false));
    }
}
