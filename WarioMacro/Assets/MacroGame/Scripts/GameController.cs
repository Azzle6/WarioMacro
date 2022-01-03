using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
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
    private static List<ITickable> tickables = new List<ITickable>();
    private static string currentScene;
    private static bool gameFinished;
    private static bool nextMicroGame;
    private static GameController instance;
    private GameState state = GameState.Micro;
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
        state = GameState.Macro;
        StartCoroutine(TickCoroutine());

    }

    private void Update()
    {
        // update global timescale
        Time.timeScale = gameSpeed / 120;
        
        // update difficulty / speed
        gameSpeed = gameControllerSO.currentGameSpeed;
        difficulty = gameControllerSO.currentDifficulty;
        
        // DEBUG Press A for next mini game
        if(state == GameState.Macro)
        {
            if (InputManager.GetKeyDown(ControllerKey.A))
            {
                Debug.Log("A Pressed, next micro game");
                nextMicroGame = true;
            }
        }
    }

    private IEnumerator TickCoroutine()
    {
        while (true)
        {
            Debug.Log(this + " => TickCoroutine: " + currentTick);
            
            foreach (var t in tickables.ToArray())
            {
                t.OnTick();
            }
            
            var asyncOp = default(AsyncOperation);
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
                    // TODO : handle player movement on game board / map
                    Debug.Log("Wait for next micro game (PRESS A)");
                }
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