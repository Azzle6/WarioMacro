using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

// ReSharper disable once CheckNamespace
public class GameController : Ticker
{
    public static GameController instance;
    [HideInInspector] public string currentScene;
    [HideInSubClass] public Player player;

    [HideInSubClass] [SerializeField] protected internal CharacterManager characterManager;
    [HideInSubClass] [SerializeField] protected internal MiniGameResultPannel_UI resultPanel;
    [HideInSubClass] [SerializeField] protected internal GameSettingsManager settingsManager;
    [HideInSubClass] [SerializeField] protected internal MapManager mapManager;
    [HideInSubClass] [SerializeField] protected internal LifeBar lifeBar;
    [HideInSubClass] [SerializeField] protected internal TextMeshProUGUI resultPanelPlaceholder;
    [HideInSubClass] [SerializeField] private RewardChart rewardChart;
    [HideInSubClass] [SerializeField] private Animator macroGameCanvasAnimator;
    [HideInSubClass] [SerializeField] private ScoreManager scoreManager;
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
    private bool stopLoop = false;
    protected internal Map map;
    private bool debugMicro;

    public delegate void InteractEvent();
    public static InteractEvent OnInteractionEnd;
    public static bool isInActionEvent;

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
            AudioManager.MacroPlaySound("GameWin", 0);
        }
        else
        {
            macroGameCanvasAnimator.SetTrigger(defeat);
            AudioManager.MacroPlaySound("GameLose", 0);
        }

        while (!InputManager.GetKeyDown(ControllerKey.A)) yield return null;
        SceneManager.LoadScene(mainMenuBuildIndex);
    }

    private IEnumerator GameLoop()
    {
        stopLoop = false;
        map = mapManager.LoadRecruitmentMap();

        MusicManager.instance.state = Soundgroup.CurrentPhase.RECRUIT;
        yield return recruitmentController.RecruitmentLoop();

        map = mapManager.LoadNextMap();
        MusicManager.instance.state = Soundgroup.CurrentPhase.ACTION;
        while(true)
        {
            yield return StartCoroutine(map.WaitForNodeSelection());

            yield return StartCoroutine(player.MoveToPosition(map.currentPath.wayPoints));
            var nodeMicroGame = map.currentNode.GetComponent<BehaviourNode>();
            

            // True if node with micro games, false otherwise
            if (nodeMicroGame != null && nodeMicroGame.enabled)
            {
                nodeMicroGame.microGamesNumber = rewardChart.GetMGNumber(MapManager.currentPhase, nodeMicroGame.behaviour);
                int[] mgDomains = nodeMicroGame.GetMGDomains();
                resultPanelPlaceholder.text = mgDomains[0].ToString(); // TODO : remove placeholder

                for (int i = 1; i < mgDomains.Length; i++)
                {
                    resultPanelPlaceholder.text += ", " + mgDomains[i];
                }

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

            var nodeInteract = map.currentNode.GetComponent<InteractibleNode>();
            if (nodeInteract != null && !isInActionEvent)
            {
                nodeInteract.EventInteractible.Invoke();
                isInActionEvent = true;
                yield return new WaitWhile(() => isInActionEvent);
            }
            

            if (map.OnLastNode())
            {
                AudioManager.MacroPlaySound("Elevator", 0);
                map = mapManager.LoadNextMap();
            }

            yield return null;
        }

        yield return astralPathController.EscapeLoop();
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
        resultPanel.Init(microGamesQueue.Count);

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
        
        map = mapManager.LoadAstralPath();
        stopLoop = true;
        return true;
    }
    
    private void OnEnable()
    {
        OnInteractionEnd += instance.InteractiveEventEnd;
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

    public void InteractiveEventEnd()
    {
        isInActionEvent = false;
    }
}
