﻿using System;
using System.Collections;
using System.Collections.Generic;
using GameTypes;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

// ReSharper disable once CheckNamespace
public class GameController : Ticker
{
    public static GameController instance;
    [HideInSubClass] public Player player;

    [HideInSubClass] [SerializeField] protected internal CharacterManager characterManager;
    [HideInSubClass] [SerializeField] protected internal MiniGameResultPannel_UI resultPanel;
    [HideInSubClass] [SerializeField] protected internal GameSettingsManager settingsManager;
    [HideInSubClass] [SerializeField] protected internal MapManager mapManager;
    [HideInSubClass] [SerializeField] private Animator macroGameCanvasAnimator;
    [HideInSubClass] [SerializeField] private ScoreManager scoreManager;
    [HideInSubClass] [SerializeField] private Alarm alarm;
    [HideInSubClass] [SerializeField] private RecruitmentController recruitmentController;
    [HideInSubClass] [SerializeField] private Timer timer;
    [HideInSubClass] [SerializeField] private TransitionController transitionController;
    [HideInSubClass] [SerializeField] private KeywordDisplay keywordManager;
    [HideInSubClass] [SerializeField] private LifeBar lifeBar;
    [HideInSubClass] [SerializeField] private int mainMenuBuildIndex;
    [SerializeField] protected internal List<GameObject> macroObjects = new List<GameObject>();
    [SerializeField] public string[] sceneNames = Array.Empty<string>();

    private static readonly int victory = Animator.StringToHash("Victory");
    private static readonly int defeat = Animator.StringToHash("Defeat");
    private static string currentScene;
    private static bool gameFinished;
    private static bool gameResult;
    protected internal Map map;
    protected internal int nodeSuccessCount;
    private bool debugMicro;


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
        map = mapManager.LoadRecruitmentMap();

        MusicManager.instance.state = Soundgroup.CurrentPhase.RECRUIT;
        yield return recruitmentController.RecruitmentLoop();

        map = mapManager.LoadNextMap();
        MusicManager.instance.state = Soundgroup.CurrentPhase.ACTION;
        while(true)
        {
            yield return StartCoroutine(map.WaitForNodeSelection());

            yield return StartCoroutine(player.MoveToPosition(map.currentPath.wayPoints));
            var nodeMicroGame = map.currentNode.GetComponent<NodeSettings>();

            // True if node with micro games, false otherwise
            if (nodeMicroGame != null)
            {
                nodeMicroGame.microGamesNumber = nodeMicroGame.type != NodeType.None &&
                                                 characterManager.SpecialistOfTypeInTeam(nodeMicroGame.type) == 0
                    ? gameControllerSO.noSpecialistMGCount
                    : gameControllerSO.defaultMGCount;
                yield return StartCoroutine(NodeWithMicroGame(nodeMicroGame));

                NodeResults(nodeMicroGame);

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
                AudioManager.MacroPlaySound("Elevator", 0);
                map = mapManager.LoadNextMap();
            }

            yield return null;
        }
    }

    protected internal IEnumerator NodeWithMicroGame(NodeSettings node)
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

            // Choose next MicroGame
            currentScene = microGamesQueue.Dequeue();
            Debug.Log("Launch Micro Game:" + currentScene);

            // Keyword trigger
            yield return keywordManager.KeyWordHandler(currentScene);

            AudioManager.MacroPlaySound("MiniGameEnter", 0);
            // Launch transition
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
            }
            else
            {
                settingsManager.DecreaseBPM();
                alarm.DecrementCount(false);
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


    }

    private void NodeResults(NodeSettings node)
    {
        scoreManager.UpdateScore(nodeSuccessCount,node.microGamesNumber,characterManager.playerTeam);

        if (node.type == NodeType.None)
        {
            NodeResultsBis(gameControllerSO.defaultIncreaseDifficultyThreshold,
                gameControllerSO.defaultDecreaseDifficultyThreshold, 
                gameControllerSO.defaultLoseCharacterThreshold);
        }
        else if (characterManager.SpecialistOfTypeInTeam(node.type) == 0)
        {
            NodeResultsBis(gameControllerSO.noSpecialistIncreaseDifficultyThreshold,
                gameControllerSO.noSpecialistDecreaseDifficultyThreshold,
                gameControllerSO.noSpecialistLoseCharacterThreshold);
        }
        else
        {
            NodeResultsBis(gameControllerSO.specialistIncreaseDifficultyThreshold, 
                gameControllerSO.specialistDecreaseDifficultyThreshold,
                gameControllerSO.specialistLoseCharacterThreshold);
        }
        
    }

    private void NodeResultsBis(int increaseDifficultyThreshold, int decreaseDifficultyThreshold, int loseCharacterThreshold)
    {
        if (nodeSuccessCount >= increaseDifficultyThreshold)
        {
            settingsManager.IncreaseDifficulty();
        }
        else if (nodeSuccessCount < decreaseDifficultyThreshold)
        {
            settingsManager.DecreaseDifficulty();

            if (!Alarm.isActive || nodeSuccessCount >= loseCharacterThreshold) return;

            lifeBar.Imprison();
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
