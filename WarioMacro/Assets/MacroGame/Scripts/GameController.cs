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

public class GameController : MonoBehaviour, ITickable
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

    private static void LoadMicroGame(string sceneName)
    {
        currentScene = sceneName;
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }

    private void Start()
    {
        SetObjActive(false);
        if (state == GameState.Macro)
        {
            LoadMicroGame(sceneNames[Random.Range(0, sceneNames.Length)]);
        }
        StartCoroutine(TickCoroutine());

    }

    private IEnumerator TickCoroutine()
    {
        while (true)
        {
            foreach (var t in tickables.ToArray())
            {
                t.OnTick();
            }
            OnTick();
            yield return new WaitForSeconds(1);
            currentTick++;
        }
    }

    public void OnTick()
    {
        //Debug.Log("Macro " + currentTick + " : " + Time.time);
        if (gameFinished && debugMicro)
        {
            ResetTick();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        /*Debug.Log("Macro" + currentTick);
        if (gameFinished)
        {
            if (state == GameState.Micro)
            {
                //SceneManager.UnloadSceneAsync(currentScene);
                ResetTick();
                SetObjActive(true);
                state = GameState.Macro;
            }
            else
            {
                if (currentTick == 8)
                {
                    ResetTick();
                    SetObjActive(false);
                    LoadMicroGame(sceneNames[Random.Range(0, sceneNames.Length)]);
                    state = GameState.Micro;
                    gameFinished = false;
                }
            }
        }*/
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

    private void Update()
    {
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