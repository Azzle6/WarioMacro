using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// ReSharper disable once CheckNamespace
public interface ITickable
{
    void OnTick();
}

public class Ticker : MonoBehaviour
{
    
    public static bool lockTimescale;
    public static float gameBPM { get; private set; }
    public static int difficulty { get; private set; }
    public static int currentTick { get; private set; }
    
    private static readonly List<ITickable> tickables = new List<ITickable>();
    private IEnumerator tickEnumerator;
    private GameConfig gameConfig;
    private MusicManager musicManager;

    public static void Init(ITickable t)
    {
        if (!tickables.Contains(t))
        {
            tickables.Add(t);
        }
    }
    
    private protected void ResetTick()
    {
        StopCoroutine(tickEnumerator);
        currentTick = 0;
        tickEnumerator = TickCoroutine();
        StartCoroutine(tickEnumerator);
    }
    
    private protected static void ResetTickables()
    {
        tickables.Clear();
    }

    private protected void TickerAwake()
    {
        gameConfig = Resources.LoadAll<GameConfig>("").First();
        gameConfig.Init();
    }

    private protected void TickerStart(bool debug)
    {
        musicManager = MusicManager.instance;
        musicManager.state = Soundgroup.CurrentPhase.RECRUIT;
        
        if (!debug)
        {
            // Reset bpm and difficulty 
            gameConfig.currentGameSpeed = gameConfig.initialGameSpeed;
            gameConfig.currentDifficulty = gameConfig.initialDifficulty;
        }
        
        // Update BPM and difficulty
        gameBPM = gameConfig.currentGameSpeed;
        difficulty = gameConfig.currentDifficulty;
        Time.timeScale = gameBPM / 60;

        tickEnumerator = TickCoroutine();
        StartCoroutine(tickEnumerator);
    }

    private protected void TickerUpdate()
    {
        // update difficulty / speed
        gameBPM = gameConfig.currentGameSpeed;
        difficulty = gameConfig.currentDifficulty;
        
        // update global timescale
        Time.timeScale = lockTimescale ? 0f : gameBPM / 60;
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

    private void Awake()
    {
        TickerAwake();
    }

    private void Start()
    {
        TickerStart(false);
    }

    private void Update()
    {
        TickerUpdate();
    }
}
