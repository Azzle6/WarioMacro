using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class NOB2_GuardBehaviour : MonoBehaviour
{
    [SerializeField] private Light2D light;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject finishLine;
    [SerializeField] private float detectionDepth;
    [SerializeField] private int detectionWidth;
    [SerializeField] private Vector3 detectionSpread;
    private bool playerFound = false;
    
    void Update()
    {
        if (!playerFound)
        {
            for (int i = 0; i < detectionWidth; i++)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, (player.transform.position - transform.position).normalized + detectionSpread * i, detectionDepth);
                if (hit.collider != null)
                {
                    Debug.DrawRay(transform.position,   new Vector3(hit.point.x - transform.position.x,hit.point.y - transform.position.y, 0), Color.green);
                    if (hit.collider.gameObject == player)
                    {
                        playerFound = true;
                    }
                }
                else
                {
                    Debug.DrawRay(transform.position, (player.transform.position - transform.position).normalized * detectionDepth, Color.red);
                }
                        
                hit = Physics2D.Raycast(transform.position, (player.transform.position - transform.position).normalized - detectionSpread * i, detectionDepth);
                if (hit.collider != null)
                {
                    Debug.DrawRay(transform.position,   new Vector3(hit.point.x - transform.position.x,hit.point.y - transform.position.y, 0), Color.green);
                    if (hit.collider.gameObject == player)
                    {
                        playerFound = true;
                    }
                }
                else
                {
                    Debug.DrawRay(transform.position, (player.transform.position - transform.position).normalized * detectionDepth, Color.red);
                }
            }
            
            if (playerFound && NOB2_GameManager.instance.resultPending)
            {
                light.color = Color.red;
                light.intensity = 3;
                NOB2_GameManager.instance.SetResult(false);
            }
        }
    }
} 
