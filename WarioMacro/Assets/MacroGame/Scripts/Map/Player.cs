using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject[] arrowPrefabs = new GameObject[4];
    
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
        //yield return new WaitForSeconds(1f);
        //map.player.transform.DOPunchScale(Vector3.one * .25f, 1f);
        //yield return new WaitForSeconds(1f);
        StartMove();
        // TODO : animate with (DOTween?)
        //map.player.transform.position = map.currentNode.transform.position;
        for (int i = 0; i < positions.Count; i++)
        {
            var tween = transform.DOMove(positions[i].position, moveSpeed).SetSpeedBased().SetEase(Ease.Linear);
            while (tween.IsPlaying()) yield return null;
        }
        

        // var positions = map.currentPath.GetPositions();
        // var tween = player.transform.DOPath((Vector3[])positions, player.moveSpeed).SetSpeedBased().SetEase(Ease.Linear);
        
        StopMove();
        AudioManager.MacroPlaySound("NodeSelect", 0);
        //yield return new WaitForSeconds(1f);
    }
    
    private void StartMove()
    {
        animator.SetTrigger(move);
    }

    private void StopMove()
    {
        animator.SetTrigger(idle);
    }
    
    private void Awake()
    {
        arrowPrefabs.ToList().ForEach(go => go.SetActive(false));
    }
}
