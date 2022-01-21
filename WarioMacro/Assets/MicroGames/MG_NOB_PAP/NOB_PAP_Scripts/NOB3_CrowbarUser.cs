using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NOB3_CrowbarUser : MonoBehaviour
{
    private bool ready = true;
    [SerializeField] private float cooldown;
    public void Use()
    {
        if (ready)
        {
            StartCoroutine(CooldownCoroutine());
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.forward);
            if (hit.collider != null)
            {
                hit.collider.GetComponent<NOB3_PlankHit>().GetHit();
            }
            else
            {
                Debug.Log(0);
            }
        }
    }

    IEnumerator CooldownCoroutine()
    {
        ready = false;
        yield return new WaitForSeconds(cooldown);
        ready = true;
    }
}
