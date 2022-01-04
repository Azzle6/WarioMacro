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

    [SerializeField] private GameControllerSO gameControllerSO;
    [SerializeField] private GameObject[] macroObjects = Array.Empty<GameObject>();
    [SerializeField] private string[] sceneNames = Array.Empty<string>();
    [SerializeField]  private GameState state = GameState.Micro;
    private Map map;
    private static List<ITickable> tickables = new List<ITickable>();
    private static string currentScene;
    private static bool gameFinished;
    private static bool nextMicroGame;
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
        tickables.Clear();
        gameFinished = false;
    }

    public static void FinishGame(bool result)
    {
        gameFinished = true;
    }

    private void Start()
    {
        StartCoroutine(TickCoroutine());
        StartCoroutine(GameStateCoroutine());
    }

    private void Update()
    {
        // update global timescale
        Time.timeScale = gameSpeed / 120;
        
        // update difficulty / speed
        gameSpeed = gameControllerSO.currentGameSpeed;
        difficulty = gameControllerSO.currentDifficulty;
    }

    private IEnumerator GameStateCoroutine()
    {
        while(true)
        {var asyncOp = default(AsyncOperation);
            if (state == GameState.Micro)
            {
                if (gameFinished)
                {
                    // Unload current micro game
                    Debug.Log("MicroGame Finished");
                    asyncOp = SceneManager.UnloadSceneAsync(currentScene);
                    while (!asyncOp.isDone) yield return null;
                    ResetTick();
                    SetObjActive(true);
                    // switch back to macro
                    state = GameState.Macro;
                    nextMicroGame = false;
                    gameFinished = false;
                }
            }
            else if (state == GameState.Macro)
            {
                // Launch next micro game
                if(nextMicroGame)
                {
                    ResetTick();
                    currentScene = sceneNames[Random.Range(0, sceneNames.Length)];
                    Debug.Log("Launch Micro Game:" + currentScene);
                    asyncOp = SceneManager.LoadSceneAsync(currentScene, LoadSceneMode.Additive);
                    while (!asyncOp.isDone) yield return null;
                    SetObjActive(false);
                    state = GameState.Micro;
                    gameFinished = false;
                    nextMicroGame = false;
                }
                else
                {
                    if (LastNodeReached() || map == null)
                    {
                        yield return StartCoroutine(LoadNextMap());
                    }

                    yield return StartCoroutine(WaitForNodeSelection());
                    yield return StartCoroutine(MovePlayerToCurrentNode());
                    nextMicroGame = map.currentNode.GetComponent<NodeTriggerMicroGame>() != null;
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
        map.player.StartMove();
        // TODO : animate with (DOTween?)
        //map.player.transform.position = map.currentNode.transform.position;
        var tween = map.player.transform.DOMove(map.currentNode.transform.position, map.player.moveSpeed)
            .SetSpeedBased()
            .SetEase(Ease.Linear);
        while (tween.IsPlaying()) yield return null;
        map.player.StopMove();
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
        map.player.transform.position = map.currentNode.transform.position;
        yield break;
    }

    private IEnumerator WaitForNodeSelection()
    {
        // init
        var arrowPrefabs = map.arrowPrefabs.ToList();
        var currentNode = map.currentNode;
        var nextNode = default(Node);
        var selectedNode = default(Node);
        var selectedDirection = -1;
        var selectInput = default(bool);
        var validInput = ControllerKey.A;
        
        // TODO : display message "select next node"
        Debug.Log("Select Next Node with Left Stick");
        
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