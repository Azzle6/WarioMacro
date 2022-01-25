using System.Collections;
using System.Linq;
using GameTypes;
using UnityEngine;

// ReSharper disable once CheckNamespace
public class RecruitmentController : GameController
{
    public bool skipRecruitment;
    public bool canFinishRecruitment;
    [SerializeField] private GameObject alarmGO;
    private NodeVisual lastNoMGNode;

    public IEnumerator RecruitmentLoop()
    {
        // For Debug purposes
        if (skipRecruitment)
        {
            yield return new WaitForSeconds(1f);
            yield return SkipRecruitment();
            yield break;
        }

        StartCoroutine(MoveLoop());
        
        
        SetRecruitmentActive(true);
        
        while(!canFinishRecruitment)
        {
            // Select path and move
            lastNoMGNode = instance.map.currentNode;
            //yield return StartCoroutine(instance.map.WaitForNodeSelection());
            
            //yield return StartCoroutine(instance.player.MoveToPosition(instance.map.currentPath.wayPoints));
            
            
            
            var typedNode = instance.map.currentNode.GetComponent<RecruitmentNode>();

            // True if node is typed, false otherwise
            if (typedNode != null)
            {
                //nodeMicroGame.microGamesNumber = instance.gameControllerSO.defaultMGCount;

                /*
                // Launch micro game loop
                yield return StartCoroutine(instance.NodeWithMicroGame(nodeMicroGame));

                yield return new WaitForSecondsRealtime(1f);
                
                // Dispose result panel
                instance.resultPanel.PopWindowDown();
                instance.resultPanel.ToggleWindow(false);
                
                // Wait for results
                yield return instance.characterManager.DisplayRecruitmentChoice(typedNode.type);
                //yield return NodeResults(nodeMicroGame);

                // Lock path if there is no character left
                if (!instance.characterManager.IsTypeAvailable(typedNode.type))
                {
                    DeletePath(instance.map.currentPath, typedNode);
                }
                
                // Return on start node
                instance.player.TeleportPlayer(startNode.transform.position);
                instance.map.currentNode = startNode;*/
            }
            StopCoroutine(MoveLoop());
            yield return null;
        }

        //yield return new WaitUntil(() => canFinishRecruitment);
        
        SetRecruitmentActive(false);
    }

    private IEnumerator MoveLoop()
    {
        while (instance.characterManager.playerTeam.Count < 4 || !canFinishRecruitment)
        {
            yield return StartCoroutine(instance.map.WaitForNodeSelection());

            yield return StartCoroutine(instance.player.MoveToPosition(instance.map.currentPath.wayPoints));
        }
    }

    private void DeletePath(NodeVisual.Path path, RecruitmentNode node)
    {
        for (var i = 0; i < lastNoMGNode.paths.Length; i++)
        {
            if (lastNoMGNode.paths[i] != path) continue;
            
            lastNoMGNode.paths[i] = null;
            break;
        }
        
        node.DisableNode();
    }

    private void SetRecruitmentActive(bool state)
    {
        /*
        if (state)
        {
            instance.macroObjects.Remove(alarmGO);
        }
        else
        {
            instance.macroObjects.Add(alarmGO);
            instance.macroObjects.Remove(nodePrevisualisationGO);
        }*/
        
        alarmGO.SetActive(!state);
    }

    public void StopRecruitPhase()
    {
        if (instance.characterManager.playerTeam.Count >= 4) canFinishRecruitment = true;
    }

    public IEnumerator SkipRecruitment()
    {
        Debug.Log("Skip Recruit");
        for (int i = 0; i < 4; i++)
        {
            instance.characterManager.Recruit(instance.characterManager.recruitableCharacters[Random.Range(0,instance.characterManager.recruitableCharacters.Count)]);
        }
        yield return null;
    }

    public void InteractiveEventEnd()
    {
        isInActionEvent = false;
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


