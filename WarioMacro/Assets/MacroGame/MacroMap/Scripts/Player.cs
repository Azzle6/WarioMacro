using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Animator animator;
    public float moveSpeed = 1f;
    [SerializeField] public GameObject[] arrowPrefabs = new GameObject[3];

    void Awake()
    {
        arrowPrefabs.ToList().ForEach(go => go.SetActive(false));
    }
    
    public void StartMove()
    {
        animator.SetTrigger("Move");
    }

    public void StopMove()
    {
        animator.SetTrigger("Idle");
    }
}
