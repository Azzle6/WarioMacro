using System.Collections;
using UnityEngine;

// ReSharper disable once CheckNamespace
public class RecruitmentController : GameController
{
    [SerializeField] private GameObject alarmGO;
    [SerializeField] private Node startNode;
    [Range(0, 3)] [SerializeField] private int askedCharacterThreshold = 2;
    [Range(0, 3)] [SerializeField] private int randomSpecialistThreshold = 1;

    private Node lastNoMGNode;

    public IEnumerator RecruitmentLoop()
    {
        SetAlarmActive(false);
        
        while(!instance.characterManager.isTeamFull)
        {
            lastNoMGNode = instance.map.currentNode;
            yield return StartCoroutine(instance.map.WaitForNodeSelection());

            yield return StartCoroutine(instance.player.MoveToPosition(instance.map.currentPath.wayPoints));
            AudioManager.MacroPlaySound("MOU_NodeSelect", 0);
            
            var nodeMicroGame = instance.map.currentNode.GetComponent<NodeSettings>();

            // True if node with micro games, false otherwise
            if (nodeMicroGame != null)
            {
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
        
        SetAlarmActive(true);
    }

    private IEnumerator NodeResults(NodeSettings node)
    {
        if (instance.nodeSuccessCount >= askedCharacterThreshold)
        {
            instance.settingsManager.IncreaseDifficulty();
            AudioManager.MacroPlaySound("MOU_NodeSuccess", 0);
            
            yield return instance.characterManager.DisplayRecruitmentChoice(node.type);
            yield break;
        }
        
        instance.settingsManager.DecreaseDifficulty();
        AudioManager.MacroPlaySound("MOU_NodeFail", 0);
        
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

    private void SetAlarmActive(bool state)
    {
        if (state)
        {
            instance.macroObjects.Add(alarmGO);
        }
        else
        {
            instance.macroObjects.Remove(alarmGO);
        }
        
        alarmGO.SetActive(state);
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


