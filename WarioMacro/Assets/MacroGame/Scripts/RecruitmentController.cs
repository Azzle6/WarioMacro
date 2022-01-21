using System.Collections;
using System.Linq;
using GameTypes;
using UnityEngine;

// ReSharper disable once CheckNamespace
public class RecruitmentController : GameController
{
    public bool skipRecruitment;
    [SerializeField] private GameObject alarmGO;
    [SerializeField] private NodePrevisualisation nodePrevisualisation;
    [SerializeField] private Node startNode;
    [Range(0, 3)] [SerializeField] private int askedCharacterThreshold = 2;
    [Range(0, 3)] [SerializeField] private int randomSpecialistThreshold = 1;

    private Node lastNoMGNode;

    public IEnumerator RecruitmentLoop()
    {
        if (skipRecruitment)
        {
            yield return new WaitForSeconds(1f);
            yield return SkipRecruitment();
            yield break;
        }
        
        SetRecruitmentActive(true);
        nodePrevisualisation.SetTexts(instance.mapManager.typePercentages.Select(pair => pair.Value).ToArray());
        
        while(!instance.characterManager.isTeamFull)
        {
            lastNoMGNode = instance.map.currentNode;
            yield return StartCoroutine(instance.map.WaitForNodeSelection());

            yield return StartCoroutine(instance.player.MoveToPosition(instance.map.currentPath.wayPoints));
            
            var nodeMicroGame = instance.map.currentNode.GetComponent<NodeSettings>();

            // True if node with micro games, false otherwise
            if (nodeMicroGame != null)
            {
                nodeMicroGame.microGamesNumber = instance.gameControllerSO.defaultMGCount;
                yield return StartCoroutine(instance.NodeWithMicroGame(nodeMicroGame));

                yield return new WaitForSecondsRealtime(1f);
                
                // dispose
                instance.resultPanel.PopWindowDown();
                instance.resultPanel.ToggleWindow(false);
                
                yield return NodeResults(nodeMicroGame);

                if (!instance.characterManager.IsTypeAvailable(nodeMicroGame.type))
                {
                    DeletePath(instance.map.currentPath);
                }
                
                instance.player.TeleportPlayer(startNode.transform.position);
                instance.map.currentNode = startNode;
            }

            yield return null;
        }
        
        SetRecruitmentActive(false);
    }

    private IEnumerator NodeResults(NodeSettings node)
    {
        if (instance.nodeSuccessCount >= askedCharacterThreshold)
        {
            instance.settingsManager.IncreaseDifficulty();

            yield return instance.characterManager.DisplayRecruitmentChoice(node.type);
            yield break;
        }
        
        instance.settingsManager.DecreaseDifficulty();

        if (instance.nodeSuccessCount >= randomSpecialistThreshold)
        {
            yield return instance.characterManager.AddDifferentSpecialist(node.type);
        }
        else
        {
            yield return instance.characterManager.AddDefaultCharacter();
        }
    }

    private void DeletePath(Node.Path path)
    {
        for (var i = 0; i < lastNoMGNode.paths.Length; i++)
        {
            if (lastNoMGNode.paths[i] != path) continue;
            
            lastNoMGNode.paths[i] = null;
            break;
        }
    }

    private void SetRecruitmentActive(bool state)
    {
        GameObject nodePrevisualisationGO = nodePrevisualisation.gameObject;
        if (state)
        {
            instance.macroObjects.Remove(alarmGO);
        }
        else
        {
            instance.macroObjects.Add(alarmGO);
            instance.macroObjects.Remove(nodePrevisualisationGO);
        }
        
        alarmGO.SetActive(!state);
        nodePrevisualisationGO.SetActive(state);
    }
    

    public IEnumerator SkipRecruitment()
    {
        Debug.Log("Skip Recruit");
        for (int i = 0; i < 4; i++)
        {
            yield return instance.characterManager.AddDifferentSpecialist(i + GameType.Brute);
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


