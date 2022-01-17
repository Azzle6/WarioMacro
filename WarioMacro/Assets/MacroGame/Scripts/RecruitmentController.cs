using System.Collections;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

// ReSharper disable once CheckNamespace
public class RecruitmentController : GameController
{
    [SerializeField] private GameObject alarmGO;
    [SerializeField] private Node startNode;
    [Range(0, 3)] [SerializeField] private int askedCharacterThreshold = 2;
    [Range(0, 3)] [SerializeField] private int randomSpecialistThreshold = 1;
    

    public IEnumerator RecruitmentLoop()
    {
        SetAlarmActive(false);
        
        while(!instance.characterManager.isTeamFull)
        {
            // TODO : Change node selection and remove 2nd no MG node from prefab
            yield return StartCoroutine(instance.map.WaitForNodeSelection());
            AudioManager.MacroPlaySound("MOU_NodeSelect", 0);

            yield return StartCoroutine(instance.player.MoveToPosition(instance.map.currentPath.wayPoints));
            
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
            yield return instance.characterManager.DisplayRecruitmentChoice(node.type);
        }
        else if (instance.nodeSuccessCount >= randomSpecialistThreshold)
        {
            yield return instance.characterManager.AddDifferentSpecialist(node.type);
        }
        else
        {
            yield return instance.characterManager.AddDefaultCharacter();
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
