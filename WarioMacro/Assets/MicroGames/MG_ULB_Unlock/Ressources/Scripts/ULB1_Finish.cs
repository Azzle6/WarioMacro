using UnityEngine;

public class ULB1_Finish : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ULB1_GameManager.instance.FinishGame(true);
        }  
    }
}
