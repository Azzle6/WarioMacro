using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

// ReSharper disable once CheckNamespace
public class GameController : Ticker
{
    public static GameController instance;
    public Player player;
    
    [SerializeField] private Animator macroGameCanvasAnimator;
    [SerializeField] private GameSettingsManager settingsManager;
    [SerializeField] private Alarm alarm;
    [SerializeField] private MapManager mapManager;
    [SerializeField] private MiniGameResultPannel_UI resultPanel;
    [SerializeField] private Timer timer;
    [SerializeField] private TransitionController transitionController;
    [SerializeField] private LifeBar lifeBar;
    [SerializeField] private int mainMenuBuildIndex;
    [SerializeField] private GameObject[] macroObjects = Array.Empty<GameObject>();
    [SerializeField] private string[] sceneNames = Array.Empty<string>();
    
    private static readonly int victory = Animator.StringToHash("Victory");
    private static readonly int defeat = Animator.StringToHash("Defeat");
    private static string currentScene;
    private static bool gameFinished;
    private static bool gameResult;
    private Map map;
    private int nodeSuccessCount;
    private bool debugMicro;
    


    public static void Register()
    {
        if (instance != null) return;
        
        new GameObject("GameController").AddComponent<GameController>();
        instance.TickerStart(true);
        instance.debugMicro = true;
        Debug.Log("macro registered");
    }
    
    public static void FinishGame(bool result)
    {
        Debug.Log("FinishGame: " + result);
        gameFinished = true;
        gameResult = result;
    }

    public void ShowMacroObjects(bool value)
    {
        foreach (GameObject obj in macroObjects)
        {
            obj.SetActive(value);
        }
    }

    
    private IEnumerator ToggleEndGame(bool value)
    {
        if (value)
        {
            macroGameCanvasAnimator.SetTrigger(victory);
            AudioManager.MacroPlaySound("MOU_GameWin", 0);
        }
        else
        {
            macroGameCanvasAnimator.SetTrigger(defeat);
            AudioManager.MacroPlaySound("MOU_GameLose", 0);
        }
        
        while (!InputManager.GetKeyDown(ControllerKey.A)) yield return null;
        SceneManager.LoadScene(mainMenuBuildIndex);
    }
    
    private IEnumerator GameLoop()
    {
        map = mapManager.LoadNextMap();
        
        while(true)
        {
            yield return StartCoroutine(map.WaitForNodeSelection());

            yield return StartCoroutine(player.MoveToPosition(map.currentNode.transform.position));
            AudioManager.MacroPlaySound("MOU_NodeSelect", 0);
            var nodeMicroGame = map.currentNode.GetComponent<NodeMicroGame>();

            // True if node with micro games, false otherwise
            if (nodeMicroGame != null)
            {
                yield return StartCoroutine(NodeWithMicroGame(nodeMicroGame));

                NodeResults();

                yield return new WaitForSecondsRealtime(1f);
                
                // dispose
                resultPanel.PopWindowDown();
                resultPanel.ToggleWindow(false);

                if (lifeBar.GetLife() == 0)
                {
                    StartCoroutine(ToggleEndGame(false));
                }
            }
            
            if (map.OnLastNode())
            {
                if (mapManager.OnLastMap())
                {
                    StartCoroutine(ToggleEndGame(true));
                    yield break;
                }
                map = mapManager.LoadNextMap();
            }

            yield return null;
        }
    }

    private IEnumerator NodeWithMicroGame(NodeMicroGame node)
    {
        // select 3 random micro games from micro games list
        var microGamesQueue = new Queue<string>();
        var microGamesList = new List<string>(sceneNames);
        var microGamesCount = Mathf.Min(node.microGamesNumber, microGamesList.Count);
        while (microGamesCount-- > 0)
        {
            var rdIndex = Random.Range(0, microGamesList.Count);
            var pickedMicroGame = microGamesList[rdIndex];
            microGamesList.RemoveAt(rdIndex);
            microGamesQueue.Enqueue(pickedMicroGame);
        }
        
        // init result panel
        resultPanel.ToggleWindow(true);
        resultPanel.SetHeaderText(MiniGameResultPannel_UI.HeaderType.GetReady);
        resultPanel.ClearAllNodes();
        resultPanel.SetStartingNodeNumber(microGamesQueue.Count);
        resultPanel.PopWindowUp();

        // play each micro games one by one
        int gameCount = 0;
        nodeSuccessCount = 0;
        while (microGamesQueue.Count > 0)
        {
            yield return new WaitForSecondsRealtime(1f);
            
            resultPanel.PopWindowDown();
            
            yield return new WaitForSeconds(1f);

            // start transition UI
            AudioManager.MacroPlaySound("MOU_MiniGameEnter", 0);
            
            // start next micro game in queue
            currentScene = microGamesQueue.Dequeue();
            Debug.Log("Launch Micro Game:" + currentScene);
            yield return StartCoroutine(transitionController.TransitionHandler(currentScene, true));

            // micro game start
            ResetTick();
            timer.StartTimer();
            gameFinished = false;
            
            // wait for game finished
            while (!gameFinished) yield return null;

            timer.StopTimer();
            yield return StartCoroutine(transitionController.TransitionHandler(currentScene, false));

            // switch back to macro state
            ResetTickables();
            ResetTick();
            
            // change BPM
            if (gameResult)
            {
                settingsManager.IncreaseBPM();
                alarm.DecrementCount(true);
                AudioManager.MacroPlaySound("MOU_SpeedUp", 0);
            }
            else
            {
                settingsManager.DecreaseBPM();
                alarm.DecrementCount(false);
                AudioManager.MacroPlaySound("MOU_SpeedDown", 0);
            }

            // display result
            //Debug.Log("MicroGame Finished: " + (gameResult ? "SUCCESS" : "FAILURE"));
            if (gameResult) nodeSuccessCount++;

            resultPanel.PopWindowUp();
            resultPanel.SetHeaderText(gameResult
                ? MiniGameResultPannel_UI.HeaderType.Success
                : MiniGameResultPannel_UI.HeaderType.Failure);
            yield return new WaitForSeconds(1f);
            
            resultPanel.SetCurrentNode(gameResult, gameCount+=1);
            yield return new WaitForSeconds(1f);
        }
        
        //Debug.Log("Node completed");
    }

    private void NodeResults()
    {
        if (nodeSuccessCount > 1)
        {
            settingsManager.IncreaseDifficulty();
            AudioManager.MacroPlaySound("MOU_NodeSuccess", 0);
        }
        else
        {
            settingsManager.DecreaseDifficulty();
            AudioManager.MacroPlaySound("MOU_NodeFail", 0);
            lifeBar.Damage();
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        TickerAwake();
    }
    
    private void Start()
    {
        if (debugMicro) return;
        
        // Init
        GameManager.Register();
        TickerStart(false);

        StartCoroutine(GameLoop());
    }

    private void Update()
    {
        TickerUpdate();
    }
}