using System.Collections;
using UnityEngine;

// ReSharper disable once CheckNamespace
public class AstralPathController : GameController
{
    [SerializeField] private GameObject alarmPostProcess;
    [SerializeField] private GameObject astralPostProcess;
    [SerializeField] private GameObject normalLights;
    [SerializeField] private GameObject astralLights;
    
    private int mgFailed;

    public IEnumerator EscapeLoop()
    {
        GameControllerSO.instance.currentDifficulty = 3;
        normalLights.SetActive(false);
        alarmPostProcess.SetActive(false);
        astralLights.SetActive(true);
        astralPostProcess.SetActive(true);
        
        yield return instance.player.ExitPortal();
        
        while (!instance.map.OnLastNode())
        {
            mgFailed = 0;
            yield return StartCoroutine(instance.map.WaitForNodeSelection());

            yield return StartCoroutine(instance.player.MoveToPosition(instance.map.currentPath.wayPoints));
            var nodeMicroGame = instance.map.currentNode.GetComponent<BehaviourNode>();
            
            if (nodeMicroGame != null && nodeMicroGame.enabled)
            {
                nodeMicroGame.microGamesNumber = GameControllerSO.instance.astralMGCount;

                yield return StartCoroutine(instance.NodeWithMicroGame(this, nodeMicroGame));

                nodeMicroGame.DisableNode();

                yield return new WaitForSecondsRealtime(1f);

                // dispose
                instance.resultPanel.PopWindowDown();
                instance.resultPanel.ToggleWindow(false);

                if (instance.lifeBar.GetLife() == 0)
                {
                    StartCoroutine(instance.ToggleEndGame(false));
                    //NotDestroyedScript.instance.EndRun(false);
                    yield break;
                }
            }
            
            yield return null;
        }
        StartCoroutine(instance.ToggleEndGame(true));
        //NotDestroyedScript.instance.EndRun(true);
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

            if (mgFailed >= GameControllerSO.instance.loseCharacterThreshold)
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
