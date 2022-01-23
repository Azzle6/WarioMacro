using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NAA2_Surveillance_MicroGameController : MonoBehaviour, ITickable
{
    public GameConfig gc;
    [SerializeField]
    NAA2_Surveillance_CameraDetect cd, cdd;
    [SerializeField]
    NAA2_Surveillance_CameraMovement cm, cmm;
    public enum Difficulty {easy, medium, hard}
    public Difficulty difficulty;

    public float tickPerSec;
    int endingTick = 5;
    public float duration;
    public float elapsedTime;
    public bool gameStarted;
    bool result;
    public bool gameOver;
    AnimationEvent victory = new AnimationEvent();

    [SerializeField]
    GameObject announcementGO;
    [SerializeField]
    Animator announcementGOanimator;
    [SerializeField]
    AnimationClip announcementAnim, victoryAnim, defeatAnim;

    [SerializeField]
    NAA2_Surveillance_Announcement announcement;

    [Header("Game Settings")]
    [SerializeField]
    float ezCamSpeed;
    [SerializeField]
    float medCamSpeed, hardCamSpeed;

    [SerializeField]
    GameObject secondCam;

    [SerializeField]
    AudioClip victorySound;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Register();
        GameController.Init(this);

        if (gc.currentDifficulty == 1) difficulty = Difficulty.easy;
        if (gc.currentDifficulty == 2) difficulty = Difficulty.medium;
        if (gc.currentDifficulty == 3) difficulty = Difficulty.hard;

        if (difficulty == Difficulty.hard)
        {
            secondCam.SetActive(true);
        }
        else
        {
            secondCam.SetActive(false);
        }
        victory.functionName = "StartDuPauvre";
        Restart();
    }

    public void OnTick()
    {
        Debug.Log(GameController.currentTick);
        if(GameController.currentTick == 5 && !gameOver)
        {
            AudioManager.PlaySound(victorySound, 0.7f, 0);
            gameStarted = false;
            announcementGO.SetActive(true);
            announcementGOanimator.Play(victoryAnim.name, 0);
            announcement.RelaunchAnnouncement();
            result = true;
            endingTick = 9;
            cd.gameObject.SetActive(false);
            cdd.gameObject.SetActive(false);
            cd.enabled = false;
            cdd.enabled = false;

            cm.stop = true;
            cmm.stop = true;

            GameController.StopTimer();
        }

        if(GameController.currentTick == 8)
        {
            GameController.FinishGame(result);
        }

        if(endingTick == GameController.currentTick)
        {
            GameController.FinishGame(result);
        }
    }
    

    // Update is called once per frame
    void Update()
    {
        if (announcement.isActiveAndEnabled)
        {
            announcementGOanimator.SetBool("defeat", cd.defeat || cdd.defeat);
        }
        if (cd.defeat ||cdd.defeat)
        {

            gameOver = true;
            gameStarted = false;
            cm.stop = true;
            cmm.stop = true;
            announcementGO.SetActive(true);
            result = false;
            announcementGOanimator.Play(defeatAnim.name, 0);

            endingTick = GameController.currentTick + 3;
            GameController.StopTimer();

        }

    }

    void Restart()
    {
        gameOver = false;
        gameStarted = true;
    }

    public void StartDuPauvre()
    {

        Debug.Log("GameEnded");

    }
}
