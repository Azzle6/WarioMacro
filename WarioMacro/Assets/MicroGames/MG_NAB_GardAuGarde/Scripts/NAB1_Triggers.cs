using UnityEngine;

public class NAB1_Triggers : MonoBehaviour
{
    public bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.transform.name);
    }
}
