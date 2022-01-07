using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public interface ITickable
{
    void OnTick();
}

public class GameController : MonoBehaviour
{
    public static int currentTick { get; private set; }
    public static float gameBPM { get; private set; }
    public static int difficulty { get; private set; }
    public Player player;

    [SerializeField] private int mainMenuBuildIndex = 0;
    [SerializeField] private Camera mainCam;
    [SerializeField] private GameControllerSO gameControllerSO;
    [SerializeField] private BPMSettingsSO bpmSettingsSO;
    [SerializeField] private GameObject[] macroObjects = Array.Empty<GameObject>();
    [SerializeField] private string[] sceneNames = Array.Empty<string>();
    [SerializeField] private MiniGameResultPannel_UI resultPanel;
    [SerializeField] private Timer timer;
    [SerializeField] private TransitionController transitionController;
    [SerializeField] private LifeBar lifeBar;
    [SerializeField] private Animator macroGameCanvasAnimator;
    [SerializeField] private MusicManager musicManager;
    
    private static readonly List<ITickable> tickables = new List<ITickable>();
    private static readonly int current = Animator.StringToHash("Current");
    private static string currentScene;
    private static bool gameFinished;
    private static bool gameResult;
    private static bool lockTimescale;
    private static GameController instance;
    private Map map;
    private float cameraHeight;
    private float cameraWidth;
    private int nodeSuccessCount;
    private bool toLaunch;
    


    public static void Register()
    {
        if (instance != null) return;
        
        new GameObject("GameController").AddComponent<GameController>();
    }

    public static void Init(ITickable t)
    {
        if (!tickables.Contains(t))
        {
            tickables.Add(t);
        }
    }
    
    public static void FinishGame(bool result)
    {
        Debug.Log("FinishGame: " + result);
        gameFinished = true;
        gameResult = result;
    }
    
    public void ToggleToLaunch(bool toggle)
    {
        toLaunch = toggle;
    }
    
    

    private static void ResetTick()
    {
        currentTick = 0;
    }

    private static void ResetTickables()
    {
        tickables.Clear();
    }

    

    private IEnumerator ToggleEndGame(bool value)
    {
        if (value)
        {
            // ReSharper disable once Unity.PreferAddressByIdToGraphicsParams
            macroGameCanvasAnimator.SetTrigger("Victory");
            MusicManager.instance.PlayASound("MOU_GameWin");
        }
        else
        {
            // ReSharper disable once Unity.PreferAddressByIdToGraphicsParams
            macroGameCanvasAnimator.SetTrigger("Defeat");
            MusicManager.instance.PlayASound("MOU_GameLose");
        }
        
        while (!InputManager.GetKeyDown(ControllerKey.A)) yield return null;
        SceneManager.LoadScene(mainMenuBuildIndex);
    }
    
    private IEnumerator GameStateCoroutine()
    {
        while(true)
        {
            if (map == null)
            {
                yield return StartCoroutine(LoadNextMap());
            }

            //Debug.Log("WaitForNodeSelection");
            
            yield return StartCoroutine(WaitForNodeSelection());
            
            //Debug.Log("MovePlayerToCurrentNode");
            
            yield return StartCoroutine(MovePlayerToCurrentNode());
            MusicManager.instance.PlayASound("MOU_NodeSelect");
            var nodeMicroGame = map.currentNode.GetComponent<NodeMicroGame>();

            // True if node with micro games, false otherwise
            if (nodeMicroGame != null)
            {
                yield return StartCoroutine(NodeWithMicroGameHandler(nodeMicroGame));
                
                ChangeDifficulty();

                // dispose
                resultPanel.PopWindowDown();
                resultPanel.ToggleWindow(false);

                if (lifeBar.GetLife() == 0)
                {
                    StartCoroutine(ToggleEndGame(false));
                }
            }
            
            if (map.currentNode == map.endNode)
            {
                StartCoroutine(ToggleEndGame(true));
            }

            yield return null;
        }
        // ReSharper disable once IteratorNeverReturns
    }
    
    private IEnumerator MovePlayerToCurrentNode()
    {
        //yield return new WaitForSeconds(1f);
        //map.player.transform.DOPunchScale(Vector3.one * .25f, 1f);
        //yield return new WaitForSeconds(1f);
        player.StartMove();
        // TODO : animate with (DOTween?)
        //map.player.transform.position = map.currentNode.transform.position;
        var tween = player.transform.DOMove(map.currentNode.transform.position, player.moveSpeed).SetSpeedBased().SetEase(Ease.Linear);

       // var positions = map.currentPath.GetPositions();
       // var tween = player.transform.DOPath((Vector3[])positions, player.moveSpeed).SetSpeedBased().SetEase(Ease.Linear);
        while (tween.IsPlaying()) yield return null;
        player.StopMove();
        //yield return new WaitForSeconds(1f);
    }

    private IEnumerator LoadNextMap()
    {
        map = FindObjectOfType<Map>();
        
        // TODO
        map.currentNode = map.startNode;
        player.transform.position = map.currentNode.transform.position;
        yield break;
    }

    private IEnumerator WaitForNodeSelection()
    {
        // init
        var arrowPrefabs = player.arrowPrefabs.ToList();
        Node currentNode = map.currentNode;
        var nextNode = default(Node);
        var nextPath = default(Node.Path);
        var selectedPath = default(Node.Path);
        var selectedNode = default(Node);
        var selectedDirection = -1;
        // ReSharper disable once TooWideLocalVariableScope
        bool selectInput;
        var lastDirectionSelected = -1;
        const ControllerKey validInput = ControllerKey.A;
        
        //Debug.Log("Select Next Node");
        
        arrowPrefabs.ForEach(go => go.SetActive(true));
        
        map.currentNode.animator.SetBool(current, true);
        
        // input loop
        while (nextNode == null)
        {
            // ReSharper disable once PossibleNullReferenceException
            foreach (Node.Path path in currentNode.paths)
            {
                var hAxis = Input.GetAxis("LEFT_STICK_HORIZONTAL");
                var vAxis = Input.GetAxis("LEFT_STICK_VERTICAL");
                var isUp = vAxis > 0.5f;
                var isDown = vAxis < -0.5f;
                var isRight = hAxis > 0.5f;
                var isLeft = hAxis < -0.5f;
                
                
                selectInput = (path.direction == Node.Direction.Up && isUp)
                              || (path.direction == Node.Direction.Down && isDown)
                              || (path.direction == Node.Direction.Left && isLeft)
                              || (path.direction == Node.Direction.Right && isRight);

                if (!selectInput) continue;
                
                selectedNode = path.destination;
                selectedPath = path;
                selectedDirection = (int)path.direction;
                if(selectedDirection != lastDirectionSelected) MusicManager.instance.PlayASound("MOU_NodeDirection");
                lastDirectionSelected = selectedDirection;
                break;
            }

            for (int i = 0; i < arrowPrefabs.Count; i++)
            {
                // is any path setup with arrow direction?
                arrowPrefabs[i].gameObject.SetActive(currentNode.paths.FirstOrDefault(p => p.direction == (Node.Direction)i) != null);
                // is the selected direction equals to the path direction?
                arrowPrefabs[i].transform.localScale = i == selectedDirection ? Vector3.one : Vector3.one * .5f;
            }
            
            if (selectedNode != null && InputManager.GetKeyDown(validInput))
            {
                nextNode = selectedNode;
                nextPath = selectedPath;
            }
            yield return null;
        }
        
        map.currentNode.animator.SetBool(current, false);
        map.currentNode = nextNode;
        map.currentPath = nextPath;
        //Debug.Log("new node selected");

        // dispose
        arrowPrefabs.ForEach(go => go.SetActive(false));
    }

    private IEnumerator NodeWithMicroGameHandler(NodeMicroGame node)
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
            yield return new WaitForSeconds(1f);
            
            resultPanel.PopWindowDown();
            
            yield return new WaitForSeconds(1f);

            // start transition UI
            MusicManager.instance.PlayASound("MOU_MiniGameEnter");
            
            // start next micro game in queue
            currentScene = microGamesQueue.Dequeue();
            Debug.Log("Launch Micro Game:" + currentScene);
            yield return StartCoroutine(TransitionHandler(true));

            // micro game start
            ResetTick();
            timer.StartTimer();
            gameFinished = false;
            
            // wait for game finished
            while (!gameFinished) yield return null;
            
            // stop timer
            timer.StopTimer();
            
            yield return StartCoroutine(TransitionHandler(false));
            
            // switch back to macro state
            ResetTickables();
            ResetTick();
            
            
            // Change BPM
            gameControllerSO.currentGameSpeed = Mathf.Clamp(
                gameBPM + (gameResult ? bpmSettingsSO.increasingBPM : -bpmSettingsSO.decreasingBPM), 
                bpmSettingsSO.minBPM, 
                bpmSettingsSO.maxBPM);
            MusicManager.instance.PlayASound(gameResult ? "MOU_SpeedUp" : "MOU_SpeedDown");
            
            
            // display result
            Debug.Log("MicroGame Finished: " + (gameResult ? "SUCCESS" : "FAILURE"));
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

    private IEnumerator TransitionHandler(bool toLoad)
    {
        // start transition UI
        transitionController.TransitionStart();
        toLaunch = false; 
        while (!toLaunch) yield return null;

        AsyncOperation asyncOp;
        if (toLoad)
        {
            ShowMacroObjects(false);
            asyncOp = SceneManager.LoadSceneAsync(currentScene, LoadSceneMode.Additive);
        }
        else
        {
            ShowMacroObjects(true);
            asyncOp = SceneManager.UnloadSceneAsync(currentScene);
        }
        
        while (!asyncOp.isDone) yield return null;
            
        // resume transition UI
        transitionController.TransitionResume();
    }

    private IEnumerator TickCoroutine()
    {
        while (true)
        {
            //Debug.Log("TICK: " + currentTick);
            foreach (ITickable t in tickables.ToArray())
            {
                t.OnTick();
            }
            musicManager.OnTick();

            yield return new WaitForSeconds(1f);
            currentTick++;
        }
        // ReSharper disable once IteratorNeverReturns
    }

    private void ChangeDifficulty()
    {
        if (nodeSuccessCount > 1)
        {
            gameControllerSO.currentDifficulty++;
            MusicManager.instance.PlayASound("MOU_NodeSuccess");
        }
        else
        {
            gameControllerSO.currentDifficulty--;
            MusicManager.instance.PlayASound("MOU_NodeFail");
            lifeBar.Damage();
        }
        gameControllerSO.currentDifficulty = Mathf.Clamp(gameControllerSO.currentDifficulty, 1, 3);
        //Debug.Log("Difficulty : " + difficulty);
    }

    private void ShowMacroObjects(bool value)
    {
        foreach (GameObject obj in macroObjects)
        {
            obj.SetActive(value);
        }
    }

    private void Awake()
    {
        instance = this;
        gameControllerSO = Resources.LoadAll<GameControllerSO>("").First();
    }
    
    private void Start()
    {
        // Init
        gameControllerSO.currentGameSpeed = 100;
        gameControllerSO.currentDifficulty = 1;
        Time.timeScale = gameBPM / 120;
        cameraHeight = 2f * mainCam.orthographicSize;
        cameraWidth = cameraHeight * mainCam.aspect;
        
        StartCoroutine(TickCoroutine());
        StartCoroutine(GameStateCoroutine());
    }

    private void Update()
    {
        // update difficulty / speed
        gameBPM = gameControllerSO.currentGameSpeed;
        difficulty = gameControllerSO.currentDifficulty;
        
        // Camera movement
        Vector3 position = player.transform.position;
        mainCam.transform.position = new Vector3(
            Mathf.Clamp(
                position.x,
                -(map.transform.GetChild(0).localScale.x/2 - cameraWidth/2),
                (map.transform.GetChild(0).localScale.x/2 - cameraWidth/2)
            ), 
            Mathf.Clamp(
                position.y,
                -(map.transform.GetChild(0).localScale.y/2 - cameraHeight/2),
                (map.transform.GetChild(0).localScale.y/2 - cameraHeight/2)
            ), 
            -10);
        
        // update global timescale
        Time.timeScale = lockTimescale ? 0f: gameBPM / 120;
    }
}