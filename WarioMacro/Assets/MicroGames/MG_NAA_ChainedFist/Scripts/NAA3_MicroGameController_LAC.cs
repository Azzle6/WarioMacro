using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NAA3_MicroGameController_LAC : MonoBehaviour, ITickable
{


    public NAA3_StickData_LAC rightStick, leftStick;

    public Vector2[] possibleStickDir;
    Vector2 validStickDir;

    public NAA3_MicroGameData_LAC MC_Data;
    float changeDirPrbPerTick;

    public int minimumBpmRequire, currentBPM;
    public GameObject[] electricLines;
    [HideInInspector]
    public bool isPlaying = true, result;

    public NAA3_UIManager_LAC uiManager;

    [Header("Sound")]
    public AudioClip[] audioClips;
    public AudioSource elecSource;
    

    private void Start()
    {
        GameManager.Register(); //Mise en place du Input Manager, du Sound Manager et du Game Controller
        GameController.Init(this); //Permet a ce script d'utiliser le OnTick

        LoadDifficultyData((GameController.difficulty-1), (GameController.gameBPM * 2)/Time.timeScale );
        ChooseStcikDir(Random.Range(0, possibleStickDir.Length - 1));

        //AudioManager.PlaySound(audioClips[0],1,);

    }

    private void Update()
    {
        elecSource.volume = Mathf.PingPong(Time.time,1);
        currentBPM = (int)currentBpm();
    }


    public void OnTick()
    {
        if (GameController.currentTick == 1)
            elecSource.enabled = true;

        if(Random.value < changeDirPrbPerTick && isPlaying)
            ChooseStcikDir(Random.Range(0, possibleStickDir.Length - 1));        

        if(GameController.currentTick == 5)
        {
            GameController.StopTimer();

            Debug.Log("Bpm : " + currentBpm() + " V_Bpm : " +minimumBpmRequire);
            Debug.Log("R_Bpm : " + rightStick.bpm + " L_Bpm : " + leftStick.bpm);
            result = CheckBpm();

            AudioManager.PlaySound(audioClips[(result)?0:1]);
            elecSource.enabled = !result;

            isPlaying = false;
        }   
        if (GameController.currentTick == 8)
            GameController.FinishGame(result);
    }

    void LoadDifficultyData(int difficulty, float bpm)
    {
        Debug.Log("Difficulty : "+difficulty);
        if (difficulty >= MC_Data.difficultySettings.Length || MC_Data.difficultySettings.Length == 0 || difficulty < 0)
            return;

        changeDirPrbPerTick = MC_Data.difficultySettings[difficulty].changeDirPrbPerTick;
        minimumBpmRequire = (int)(bpm * MC_Data.baseBpmMult * MC_Data.difficultySettings[difficulty].BpmMult);
        for(int i = 0; i < electricLines.Length; i++)
        {
            if (i > difficulty)
                electricLines[i].SetActive(false);
        }
    }

    void ChooseStcikDir(int dirIndex)
    {
        if (dirIndex >= possibleStickDir.Length)
            return;

        validStickDir = possibleStickDir[dirIndex];

        if(validStickDir != rightStick.validStickDir)
            AudioManager.PlaySound(audioClips[2]);

        rightStick.validStickDir = validStickDir;
        leftStick.validStickDir = new Vector2(-validStickDir.x, validStickDir.y);

        
    }

    bool CheckBpm()
    {
        return (minimumBpmRequire < currentBpm());
    }

    public float currentBpm()
    {
        return ((leftStick.bpm + rightStick.bpm) * 0.5f);
    }


    
    
}

