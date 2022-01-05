using UnityEngine;

public class ULB_Unlock_Finish : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ULB_Unlock_GameManager.instance.FinishGame(true);
        }  
    }
}
