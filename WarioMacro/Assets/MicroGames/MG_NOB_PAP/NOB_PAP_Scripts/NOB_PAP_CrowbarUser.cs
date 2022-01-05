using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NOB_PAP_CrowbarUser : MonoBehaviour
{
    private bool ready = true;
    [SerializeField] private float cooldown;
    [SerializeField] private GameObject fxPrefab;
    public void Use()
    {
        if (ready)
        {
            StartCoroutine(CooldownCoroutine());
            if (fxPrefab != null)
            {
                Instantiate(fxPrefab, transform.position, transform.rotation);
            }
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.forward);
            if (hit.collider != null)
            {
                hit.collider.GetComponent<NOB_PAP_PlankHit>().GetHit();
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
