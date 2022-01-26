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
    [HideInInspector] public string currentScene;
    [HideInSubClass] public Player player;

    
    [HideInSubClass] public ScoreManager scoreManager;
    [HideInSubClass] public HallOfFame hallOfFame;
    [HideInSubClass] [SerializeField] protected internal CharacterManager characterManager;
    [HideInSubClass] [SerializeField] protected internal MiniGameResultPannel_UI resultPanel;
    [HideInSubClass] [SerializeField] protected internal GameSettingsManager settingsManager;
    [HideInSubClass] [SerializeField] protected internal MapManager mapManager;
    [HideInSubClass] [SerializeField] protected internal LifeBar lifeBar;
    [HideInSubClass] [SerializeField] private RewardChart rewardChart;
    [HideInSubClass] [SerializeField] private Animator macroGameCanvasAnimator;
    [HideInSubClass] [SerializeField] private Alarm alarm;
    [HideInSubClass] [SerializeField] private RecruitmentController recruitmentController;
    [HideInSubClass] [SerializeField] private AstralPathController astralPathController;
    [HideInSubClass] [SerializeField] private Timer timer;
    [HideInSubClass] [SerializeField] private MenuManager menu;
    [HideInSubClass] [SerializeField] private TransitionController transitionController;
    [HideInSubClass] [SerializeField] private KeywordDisplay keywordManager;
    [HideInSubClass] [SerializeField] private int mainMenuBuildIndex;
    [SerializeField] protected internal List<GameObject> macroObjects = new List<GameObject>();
    [SerializeField] public string[] sceneNames = Array.Empty<string>();

    private static readonly int victory = Animator.StringToHash("Victory");
    private static readonly int defeat = Animator.StringToHash("Defeat");
    private static bool gameFinished;
    private static bool gameResult;
    public bool stopLoop;
    protected internal Map map;
    private bool debugMicro;


    public bool runChronometer = false;
    public float chronometer;
    public float startTimer;
    public delegate void InteractEvent();
    public static InteractEvent OnInteractionEnd;
    public static bool isInActionEvent;

    public bool WantToContinue;

    public static void Register()
    {
        if (instance != null) return;
        new GameObject("GameController").AddComponent<GameController>();
        instance.TickerStart(true);
        instance.debugMicro = true;
        Debug.Log("macro registered");
        
    }

    public static void StopTimer()
    {
        instance.timer.PauseTimer();
    }

    public static void FinishGame(bool result)
    {
        // Debug.Log("FinishGame: " + result);
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

    internal IEnumerator ToggleEndGame(bool value)
    {
        if (value)
        {
            macroGameCanvasAnimator.SetTrigger(victory);
            scoreManager.AddToCurrentMoney();
            AudioManager.MacroPlaySound("GameWin", 0);
        }
        else
        {
            macroGameCanvasAnimator.SetTrigger(defeat);
            AudioManager.MacroPlaySound("GameLose", 0);
        }

        instance.hallOfFame.UpdateHallOfFame(GameController.instance.scoreManager.currentRunMoney,GameController.instance.chronometer);
        characterManager.ResetEndGame();
        PlayerPrefs.Save();
        while (!InputManager.GetKeyDown(ControllerKey.A)) yield return null;
        //NotDestroyedScript.isAReload = true;
        AsyncOperation asyncLoadLvl = SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
        while (!asyncLoadLvl.isDone) yield return null;
        
        CharacterManager.IsFirstLoad = false;
        //Debug.Log("Oui");
        runChronometer = false;
    }

    private void InitHeist()
    {
        Debug.Log("Phase braquage");
        map = mapManager.LoadNextMap();
        MusicManager.instance.state = Soundgroup.CurrentPhase.ACTION;
        startTimer = Time.unscaledTime;
        runChronometer = true;
        Alarm.isActive = false;
        scoreManager.ShowMoney();
    }

    private IEnumerator GameLoop()
    {
        stopLoop = false;
        map = mapManager.LoadRecruitmentMap();

        MusicManager.instance.state = Soundgroup.CurrentPhase.RECRUIT;
        yield return recruitmentController.RecruitmentLoop();

        InitHeist();
        
        while(true)
        {
            yield return StartCoroutine(map.WaitForNodeSelection());

            if (stopLoop)
            {
                break;
            }
            Debug.Log("NodeSelect");
            
            yield return StartCoroutine(player.MoveToPosition(map.currentPath.wayPoints));
            Debug.Log("EndDisplacement");
            var nodeMicroGame = map.currentNode.GetComponent<BehaviourNode>();
            

            // True if node with micro games, false otherwise
            if (nodeMicroGame != null && nodeMicroGame.enabled)
            {
                nodeMicroGame.microGamesNumber = rewardChart.GetMGNumber(MapManager.currentPhase, nodeMicroGame.behaviour);

                yield return StartCoroutine(NodeWithMicroGame(this, nodeMicroGame));

                nodeMicroGame.DisableNode();

                yield return new WaitForSecondsRealtime(1f);

                // dispose
                resultPanel.PopWindowDown();
                resultPanel.ToggleWindow(false);

                if (stopLoop)
                {
                    break;
                }
                
            }

            /*var nodeInteract = map.currentNode.GetComponent<InteractibleNode>();
            if (nodeInteract != null && !isInActionEvent)
            {
                nodeInteract.EventInteractible.Invoke();
                isInActionEvent = true;
                yield return new WaitWhile(() => isInActionEvent);
            }*/
            

            if (map.OnLastNode())
            {
                isInActionEvent = true;
                yield return StartCoroutine(ElevatorTrigger());
                if (WantToContinue)
                {
                    AudioManager.MacroPlaySound("Elevator", 0);
                    map = mapManager.LoadNextMap();
                    map.currentNode = map.startNode;
                }
                WantToContinue = false;
            }

            yield return null;
        }

        yield return new WaitForSecondsRealtime(2f);

        yield return player.EnterPortal();

        map = mapManager.LoadAstralPath();
        yield return astralPathController.EscapeLoop();
    }

    private IEnumerator ElevatorTrigger()
    {
        map.currentNode.gameObject.GetComponent<InteractibleNode>().EventInteractible.Invoke();

        yield return new WaitWhile(() => isInActionEvent);
        
        yield return null;
    }

    public void NextMap()
    {
        AudioManager.MacroPlaySound("Elevator", 0);
        map = mapManager.LoadNextMap();
    }

    internal IEnumerator NodeWithMicroGame(GameController controller, BehaviourNode behaviourNode)
    {
        // select 3 random micro games from micro games list
        var microGamesQueue = new Queue<string>();
        var microGamesList = new List<string>(sceneNames);
        int currentMG = 0;

        for (int i = Mathf.Min(behaviourNode.microGamesNumber, microGamesList.Count) - 1; i >= 0; i--)
        {
            var rdIndex = Random.Range(0, microGamesList.Count);
            microGamesQueue.Enqueue(microGamesList[rdIndex]);
            microGamesList.RemoveAt(rdIndex);
        }

        // init result panel
        resultPanel.Init(microGamesQueue.Count, behaviourNode.GetMGDomains());
        yield return new WaitForSeconds(1f);

        // play each micro games one by one
        while (microGamesQueue.Count > 0)
        {
            yield return new WaitForSecondsRealtime(1f);

            resultPanel.PopWindowDown();

            yield return new WaitForSeconds(1f);

            // Choose next MicroGame
            currentScene = microGamesQueue.Dequeue();
            Debug.Log("Launch Micro Game:" + currentScene);

            // Keyword trigger
            yield return keywordManager.KeyWordHandler(currentScene);

            // Disable menu
            menu.enabled = false;
            
            // Launch transition
            AudioManager.MacroPlaySound("MiniGameEnter", 0);
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
            menu.enabled = true;
            ResetTickables();
            ResetTick();

            if (controller.MGResults(behaviourNode, currentMG, gameResult)) 
                yield break;

            resultPanel.PopWindowUp();
            resultPanel.SetHeaderText(gameResult);
            yield return new WaitForSeconds(1f);
        
            resultPanel.SetCurrentNode(gameResult);
            currentMG++;
            yield return new WaitForSeconds(1f);
        }
    }

    protected virtual bool MGResults(BehaviourNode behaviourNode, int mgNumber, bool result)
    {
        // change BPM and alarm or money
        if (result)
        {
            settingsManager.IncreaseBPM();
            scoreManager.AddMoney(rewardChart.GetMoneyBags(MapManager.currentPhase, behaviourNode.behaviour) *
                                  (characterManager.SpecialistOfTypeInTeam(behaviourNode.GetMGDomain(mgNumber)) > 0 ? 2 : 1));
        }
        else
        {
            settingsManager.DecreaseBPM();
            alarm.DecrementCount(MapManager.currentPhase, behaviourNode.behaviour);
        }

        if (!Alarm.isActive) return false;
        
        stopLoop = true;
        return true;
    }
    

    private void Awake()
    {
        
        if (instance == null)
        {
            instance = this;
            OnInteractionEnd += instance.InteractiveEventEnd;
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
        if (runChronometer)
            chronometer = Time.unscaledTime - startTimer;
        else
            chronometer = 0;

    }

    public void InteractiveEventEnd()
    {
        isInActionEvent = false;
    }
}
