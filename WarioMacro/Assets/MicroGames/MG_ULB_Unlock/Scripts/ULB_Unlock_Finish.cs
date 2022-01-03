using UnityEngine;

public class ULB_Unlock_Finish : MonoBehaviour
{
    [SerializeField] private ULB_Unlock_GameManager gameManager;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.FinishGame(true);
        }  
    }
}
