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
    [SerializeField] private NodeVisual startNode;
    [Range(0, 3)] [SerializeField] private int askedCharacterThreshold = 2;
    [Range(0, 3)] [SerializeField] private int randomSpecialistThreshold = 1;

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
        
        // Initialise recruitment
        SetRecruitmentActive(true);
        nodePrevisualisation.SetTexts(instance.mapManager.typePercentages.Select(pair => pair.Value).ToArray());
        
        while(!instance.characterManager.isTeamFull)
        {
            // Select path and move
            lastNoMGNode = instance.map.currentNode;
            yield return StartCoroutine(instance.map.WaitForNodeSelection());

            if (instance.characterManager.isTeamFull)
            {
                SetRecruitmentActive(false);
                yield break;
            }

            yield return StartCoroutine(instance.player.MoveToPosition(instance.map.currentPath.wayPoints));
            
            var typedNode = instance.map.currentNode.GetComponent<RecruitmentNode>();

            // True if node is typed, false otherwise
            if (typedNode != null)
            {
                //nodeMicroGame.microGamesNumber = instance.gameControllerSO.defaultMGCount;
                
                if (nodePrevisualisation.onScreen)
                {
                    nodePrevisualisation.Show();
                }
                nodePrevisualisation.enabled = false;
                
                /*
                // Launch micro game loop
                yield return StartCoroutine(instance.NodeWithMicroGame(nodeMicroGame));

                yield return new WaitForSecondsRealtime(1f);
                
                // Dispose result panel
                instance.resultPanel.PopWindowDown();
                instance.resultPanel.ToggleWindow(false);
                
                */
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
                instance.map.currentNode = startNode;
                nodePrevisualisation.enabled = true;
            }

            yield return null;
        }
        
        SetRecruitmentActive(false);
    }

    private IEnumerator NodeResults(RecruitmentNode recruitmentNode)
    {
        // If player won enough, let him choice a character
        if (instance.nodeSuccessCount >= askedCharacterThreshold)
        {
            instance.settingsManager.IncreaseDifficulty();

            yield return instance.characterManager.DisplayRecruitmentChoice(recruitmentNode.type);
            yield break;
        }
        
        instance.settingsManager.DecreaseDifficulty();

        // Partial lost, give him a random character
        if (instance.nodeSuccessCount >= randomSpecialistThreshold)
        {
            yield return instance.characterManager.AddDifferentSpecialist(recruitmentNode.type);
        }
        // Completely lost, give him a scoundrel
        else
        {
            //yield return instance.characterManager.AddDefaultCharacter();
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
        GameObject nodePrevisualisationGO = nodePrevisualisation.gameObject;
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
        nodePrevisualisationGO.SetActive(state);
    }
    

    public IEnumerator SkipRecruitment()
    {
        Debug.Log("Skip Recruit");
        for (int i = 0; i < 4; i++)
        {
            yield return instance.characterManager.AddDifferentSpecialist(i + SpecialistType.Brute);
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


