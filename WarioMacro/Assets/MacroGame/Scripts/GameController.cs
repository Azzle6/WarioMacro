﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public interface ITickable
{
    void OnTick();
}

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public static bool lockTimescale;
    public static int currentTick { get; private set; }
    public static float gameBPM { get; private set; }
    public static int difficulty { get; private set; }
    public Player player;
    

    [SerializeField] private GameControllerSO gameControllerSO;
    [SerializeField] private BPMSettingsSO bpmSettingsSO;
    [SerializeField] private Camera mainCam;
    [SerializeField] private Animator macroGameCanvasAnimator;
    [SerializeField] private MapManager mapManager;
    [SerializeField] private MiniGameResultPannel_UI resultPanel;
    [SerializeField] private Timer timer;
    [SerializeField] private TransitionController transitionController;
    [SerializeField] private LifeBar lifeBar;
    [SerializeField] private int mainMenuBuildIndex;
    [SerializeField] private GameObject[] macroObjects = Array.Empty<GameObject>();
    [SerializeField] private string[] sceneNames = Array.Empty<string>();

    private static readonly List<ITickable> tickables = new List<ITickable>();
    private static string currentScene;
    private static bool gameFinished;
    private static bool gameResult;
    private IEnumerator tickEnumerator;
    private MusicManager musicManager;
    private Map map;
    private float cameraHeight;
    private float cameraWidth;
    private int nodeSuccessCount;
    


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
    
    
    public void ShowMacroObjects(bool value)
    {
        foreach (GameObject obj in macroObjects)
        {
            obj.SetActive(value);
        }
    }




    private static void ResetTickables()
    {
        tickables.Clear();
    }

    private void ResetTick()
    {
        StopCoroutine(tickEnumerator);
        currentTick = 0;
        tickEnumerator = TickCoroutine();
        StartCoroutine(tickEnumerator);
    }

    private IEnumerator ToggleEndGame(bool value)
    {
        if (value)
        {
            // ReSharper disable once Unity.PreferAddressByIdToGraphicsParams
            macroGameCanvasAnimator.SetTrigger("Victory");
            AudioManager.MacroPlaySound("MOU_GameWin", 0);
        }
        else
        {
            // ReSharper disable once Unity.PreferAddressByIdToGraphicsParams
            macroGameCanvasAnimator.SetTrigger("Defeat");
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
                yield return StartCoroutine(NodeWithMicroGameHandler(nodeMicroGame));
                
                ChangeDifficulty();

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
            
            
            // Change BPM
            gameControllerSO.currentGameSpeed = Mathf.Clamp(
                gameBPM + (gameResult ? bpmSettingsSO.increasingBPM : -bpmSettingsSO.decreasingBPM), 
                bpmSettingsSO.minBPM, 
                bpmSettingsSO.maxBPM);
            AudioManager.MacroPlaySound(gameResult ? "MOU_SpeedUp" : "MOU_SpeedDown", 0);
            
            
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
            AudioManager.MacroPlaySound("MOU_NodeSuccess", 0);
        }
        else
        {
            gameControllerSO.currentDifficulty--;
            AudioManager.MacroPlaySound("MOU_NodeFail", 0);
            lifeBar.Damage();
        }
        gameControllerSO.currentDifficulty = Mathf.Clamp(gameControllerSO.currentDifficulty, 1, 3);
        //Debug.Log("Difficulty : " + difficulty);
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
        gameControllerSO = Resources.LoadAll<GameControllerSO>("").First();
    }
    
    private void Start()
    {
        GameManager.Register();
        // Init
        gameControllerSO.currentGameSpeed = 100;
        gameControllerSO.currentDifficulty = 1;
        Time.timeScale = gameBPM / 120;
        
        cameraHeight = 2f * mainCam.orthographicSize;
        cameraWidth = cameraHeight * mainCam.aspect;
        
        musicManager = MusicManager.instance;

        tickEnumerator = TickCoroutine();
        StartCoroutine(tickEnumerator);
        StartCoroutine(GameLoop());
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