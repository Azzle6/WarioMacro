using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NOB_PAP_PlankManager : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text resultText;
    [SerializeField] private int difficulty;
    
    [SerializeField] private List<GameObject> planks;
    
    [SerializeField] private float smallPlankProbability;
    private Vector3 smallPlankScale;
    
    [SerializeField] private float maxRotation;
    [SerializeField] private float minSpawnHeight;
    [SerializeField] private float maxSpawnHeight;
    
    private int planksToPlace;
    public int planksLeft;
    void Start()
    {
        difficulty = GameController.difficulty;
        switch (difficulty)
        {
            case 1:
                planksToPlace = 3;
                break;
            case 2:
                planksToPlace = 4;
                break;
            case 3:
                planksToPlace = 5;
                break;
            default:
                planksToPlace = 0;
                Debug.Log("Invalid Difficulty");
                break;
        }
        planksLeft = planksToPlace;

        smallPlankScale = new Vector3(0.5f, 0.5f, 0.5f);
        for (int i = 0; i < planksToPlace; i++)
        {
            planks[i].SetActive(true);
            
            if (Random.Range(0f, 1f) <= smallPlankProbability)
            {
                planks[i].transform.localScale = smallPlankScale;
                if (Random.Range(0, 2) == 1)
                {
                    planks[i].transform.position = new Vector3(1.5f, Random.Range(minSpawnHeight, maxSpawnHeight), 0);
                }
                else
                {
                    planks[i].transform.position = new Vector3(-1.5f, Random.Range(minSpawnHeight, maxSpawnHeight), 0);
                }
            }
            else
            {
                planks[i].transform.position = new Vector3(0, Random.Range(minSpawnHeight, maxSpawnHeight), 0);
            }
            planks[i].transform.Rotate(Vector3.forward, Random.Range(-maxRotation, maxRotation));
        }
    }

    public void DistachPlanch()
    {
        planksLeft--;
    }
}
