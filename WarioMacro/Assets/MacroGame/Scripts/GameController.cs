using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
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
    private static float gameSpeed { get; set; }
    public static int difficulty { get; private set; }

    
    [SerializeField] public Player player;
    [SerializeField] private GameControllerSO gameControllerSO;
    [SerializeField] private GameObject[] macroObjects = Array.Empty<GameObject>();
    [SerializeField] private string[] sceneNames = Array.Empty<string>();
    [SerializeField] private GameState state = GameState.Micro;
    [SerializeField] private MiniGameResultPannel_UI resultPanel = null;
    private Map map;
    private static List<ITickable> tickables = new List<ITickable>();
    private static string currentScene;
    private static bool gameFinished;
    private static bool gameResult;
    private static bool lockTimescale;
    private static GameController instance;
    private bool debugMicro;


    public static void Register()
    {
        if (instance == null)
        {
            new GameObject("GameController").AddComponent<GameController>();
            instance.debugMicro = true;
        }

    }

    public static void Init(ITickable t)
    {
        if (!tickables.Contains(t))
        {
            tickables.Add(t);
        }
    }

    private static void ResetTick()
    {
        currentTick = 0;
    }

    private static void ResetTickables()
    {
        tickables.Clear();
    }

    public static void FinishGame(bool result)
    {
        Debug.Log("FinishGame: " + result);
        gameFinished = true;
        gameResult = result;
    }

    private void Start()
    {
        StartCoroutine(TickCoroutine());
        StartCoroutine(GameStateCoroutine());
    }

    private void Update()
    {
        // update difficulty / speed
        gameSpeed = gameControllerSO.currentGameSpeed;
        difficulty = gameControllerSO.currentDifficulty;
        
        // update global timescale
        Time.timeScale =lockTimescale ? 0f: gameSpeed / 120;
    }

    private IEnumerator GameStateCoroutine()
    {
        var asyncOp = default(AsyncOperation);
        
        while(true)
        {
            if (state == GameState.Micro)
            {
                // Game launched from Micro Game Scene
            }
            else if (state == GameState.Macro)
            {
                if (LastNodeReached() || map == null)
                {
                    yield return StartCoroutine(LoadNextMap());
                }

                Debug.Log("WaitForNodeSelection");
                yield return StartCoroutine(WaitForNodeSelection());
                
                Debug.Log("MovePlayerToCurrentNode");
                yield return StartCoroutine(MovePlayerToCurrentNode());

                var nextMicroGame = map.currentNode.GetComponent<NodeTriggerMicroGame>();
                if(nextMicroGame != null)
                {
                    // select 3 random micro games from micro games list
                    var microGamesQueue = new Queue<string>();
                    var microGamesList = new List<string>(sceneNames);
                    var microGamesCount = Mathf.Min(3, microGamesList.Count);
                    while (microGamesCount-- > 0)
                    {
                        var rdIndex = Random.Range(0, microGamesList.Count);
                        var pickedMicroGame = microGamesList[rdIndex];
                        microGamesList.RemoveAt(rdIndex);
                        microGamesQueue.Enqueue(pickedMicroGame);
                    }
                    
                    // init result panel
                    resultPanel.ToggleWindow(true);
                    resultPanel.ClearAllNodes();
                    resultPanel.SetStartingNodeNumber(microGamesQueue.Count);
                    resultPanel.PopWindowUp();

                    // play each micro games one by one
                    int gameCount = 0;
                    while (microGamesQueue.Count > 0)
                    {
                        // wait for input pressed
                        while (true)
                        {
                            if (InputManager.GetKeyDown(ControllerKey.A))
                                break;
                            else
                                yield return null;
                        }
                        
                        resultPanel.PopWindowDown();
                        yield return new WaitForSeconds(1f);
                        
                        // start next micro game in queue
                        currentScene = microGamesQueue.Dequeue();
                        Debug.Log("Launch Micro Game:" + currentScene);
                        
                        // load scene
                        asyncOp = SceneManager.LoadSceneAsync(currentScene, LoadSceneMode.Additive);
                        while (!asyncOp.isDone) yield return null;
                        
                        // switch micro game state
                        SetObjActive(false);
                        ResetTick();
                        state = GameState.Micro;
                        
                        // wait for game finished
                        gameFinished = false;
                        while (!gameFinished) yield return null;
                        
                        // unload scene
                        asyncOp = SceneManager.UnloadSceneAsync(currentScene);
                        while (!asyncOp.isDone) yield return null;
                        
                        // switch back to macro state
                        SetObjActive(true);
                        ResetTickables();
                        ResetTick();
                        state = GameState.Macro;
                        
                        // display result
                        Debug.Log("MicroGame Finished: " + (gameResult ? "SUCCESS" : "FAILURE"));
                        
                        resultPanel.PopWindowUp();
                        yield return new WaitForSeconds(1f);
                        
                        resultPanel.SetCurrentNode(gameResult, gameCount+=1);
                        yield return new WaitForSeconds(1f);
                    }
                    
                    Debug.Log("Node completed");
                    
                    // dispose
                    resultPanel.PopWindowDown();
                    resultPanel.ToggleWindow(false);
                }
            }

            yield return null;
        }
    }
    
    private IEnumerator MovePlayerToCurrentNode()
    {
        //yield return new WaitForSeconds(1f);
        //map.player.transform.DOPunchScale(Vector3.one * .25f, 1f);
        //yield return new WaitForSeconds(1f);
        player.StartMove();
        // TODO : animate with (DOTween?)
        //map.player.transform.position = map.currentNode.transform.position;
        var tween = player.transform.DOMove(map.currentNode.transform.position, player.moveSpeed)
            .SetSpeedBased()
            .SetEase(Ease.Linear);
        while (tween.IsPlaying()) yield return null;
        player.StopMove();
        //yield return new WaitForSeconds(1f);
    }

    private bool LastNodeReached()
    {
        return map != null && map.currentNode == map.endNode;
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
        var currentNode = map.currentNode;
        var nextNode = default(Node);
        var selectedNode = default(Node);
        var selectedDirection = -1;
        var selectInput = default(bool);
        var validInput = ControllerKey.A;
        
        // TODO : display message "select next node"
        Debug.Log("Select Next Node");
        
        arrowPrefabs.ForEach((go => go.SetActive(true)));
        
        map.currentNode.animator.SetBool("Current", true);
        
        // input loop
        while (nextNode == null)
        {
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

                if (selectInput)
                {
                    selectedNode = path.destination;
                    selectedDirection = (int)path.direction;
                    break;
                }
            }

            for (int i = 0; i < arrowPrefabs.Count; i++)
            {
                // is any path setup with arrow direction?
                arrowPrefabs[i].gameObject.SetActive(currentNode.paths.FirstOrDefault(p => p.direction == (Node.Direction)i) != null);
                // is the selected direction equals to the path direction?
                arrowPrefabs[i].transform.localScale = i == (int) selectedDirection ? Vector3.one : Vector3.one * .5f;
            }
            
            if (selectedNode != null)
            {
                if (InputManager.GetKeyDown(validInput))
                {
                    nextNode = selectedNode;
                }
            }
            yield return null;
        }
        
        map.currentNode.animator.SetBool("Current", false);
        map.currentNode = nextNode;
        Debug.Log("new node selected");

        // dispose
        arrowPrefabs.ForEach((go => go.SetActive(false)));
    }

    private IEnumerator TickCoroutine()
    {
        while (true)
        {
            //Debug.Log("TICK: " + currentTick);
            foreach (var t in tickables.ToArray())
            {
                t.OnTick();
            }

            yield return new WaitForSeconds(1f);
            currentTick++;
        }
    }

    private void SetObjActive(bool value)
    {
        foreach (var obj in macroObjects)
        {
            obj.SetActive(value);
        }
    }

    private void Awake()
    {
        instance = this;
        gameControllerSO = Resources.LoadAll<GameControllerSO>("").First();
        gameSpeed = gameControllerSO.currentGameSpeed;
        difficulty = gameControllerSO.currentDifficulty;

        Time.timeScale = gameSpeed / 120;
    }

    private enum GameState
    {
        Macro,
        Micro
    }
}