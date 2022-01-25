using System.Collections;
using UnityEngine;

// ReSharper disable once CheckNamespace
public class AstralPathController : GameController
{
    private int mgFailed;

    public IEnumerator EscapeLoop()
    {
        GameConfig.instance.currentDifficulty = 3;
        yield return instance.player.ExitPortal();
        
        while (!instance.map.OnLastNode())
        {
            mgFailed = 0;
            yield return StartCoroutine(instance.map.WaitForNodeSelection());

            yield return StartCoroutine(instance.player.MoveToPosition(instance.map.currentPath.wayPoints));
            var nodeMicroGame = instance.map.currentNode.GetComponent<BehaviourNode>();
            
            if (nodeMicroGame != null && nodeMicroGame.enabled)
            {
                nodeMicroGame.microGamesNumber = GameConfig.instance.astralMGCount;
                int[] mgDomains = nodeMicroGame.GetMGDomains();
                instance.resultPanelPlaceholder.text = mgDomains[0].ToString(); // TODO : remove placeholder

                for (int i = 1; i < mgDomains.Length; i++)
                {
                    instance.resultPanelPlaceholder.text += ", " + mgDomains[i];
                }

                yield return StartCoroutine(instance.NodeWithMicroGame(this, nodeMicroGame));

                nodeMicroGame.DisableNode();

                yield return new WaitForSecondsRealtime(1f);

                // dispose
                instance.resultPanel.PopWindowDown();
                instance.resultPanel.ToggleWindow(false);

                if (instance.lifeBar.GetLife() == 0)
                {
                    StartCoroutine(instance.ToggleEndGame(false));
                    yield break;
                }
            }
            
            yield return null;
        }
        StartCoroutine(instance.ToggleEndGame(true));
    }

    protected override bool MGResults(BehaviourNode behaviourNode, int mgNumber, bool result)
    {
        if (result)
        {
            instance.settingsManager.IncreaseBPM();
        }
        else
        {
            instance.settingsManager.DecreaseBPM();
            mgFailed++;

            if (mgFailed >= GameConfig.instance.loseCharacterThreshold)
            {
                instance.characterManager.LoseCharacter();
                return true;
            }
        }

        return false;
    }
    
    // Calling event functions to hide GameController's ones
    private void OnEnable()
    {
    }
    
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
