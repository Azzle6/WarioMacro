using System.Collections;
using UnityEngine;

// ReSharper disable once CheckNamespace
public class RecruitmentController : GameController
{
    public bool skipRecruitment;
    public bool canFinishRecruitment;
    [SerializeField] private GameObject alarmGO;

    public IEnumerator RecruitmentLoop()
    {
        SetRecruitmentActive(true);
        
        // For Debug purposes
        if (skipRecruitment)
        {
            SkipRecruitment();
            SetRecruitmentActive(false);
            yield break;
        }

        StartCoroutine(MoveLoop());
        
        
        SetRecruitmentActive(true);
        
        while(!canFinishRecruitment)
        {
            // Select path and move
            var typedNode = instance.map.currentNode.GetComponent<RecruitmentNode>();


            yield return null;
        }
        StopCoroutine(MoveLoop());
    }

    private IEnumerator MoveLoop()
    {
        while (instance.characterManager.playerTeam.Count < 4 || !canFinishRecruitment)
        {
            yield return StartCoroutine(instance.map.WaitForNodeSelection());

            yield return StartCoroutine(instance.player.MoveToPosition(instance.map.currentPath.wayPoints));
        }
    }

    private void SetRecruitmentActive(bool state)
    {
        alarmGO.SetActive(!state);
    }

    public void StopRecruitPhase()
    {
        if (instance.characterManager.playerTeam.Count >= 4) canFinishRecruitment = true;
    }

    public void SkipRecruitment()
    {
        Debug.Log("Skip Recruitment");
        for (int i = 0; i < 4; i++)
        {
            instance.characterManager.Recruit(instance.characterManager.recruitableCharacters[Random.Range(0,instance.characterManager.recruitableCharacters.Count)]);
        }
    }


    // Calling event functions to hide GameController's ones
    private void Awake()
    {
    }

    private void Start()
    {
    }

    private void Update()
    {
    }
}


