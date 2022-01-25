using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ULC4_GameManager : MonoBehaviour, ITickable
{
    public static ULC4_GameManager instance;
    public bool inGame;
    private bool result;
    
    [SerializeField] private ULC4_Enemy enemy;
    
    [SerializeField] private Image winImg, loseImg;
    [SerializeField] private List<GameObject> sceneObjects;

    [SerializeField] private AudioClip victorySound, failureSound;
    
    private int endTick = -1;
    
    void Awake()
    {
        instance = this;
        inGame = true;
    }
    
    void Start()
    {
        GameManager.Register();
        GameController.Init(this);

        if (GameController.difficulty == 1) enemy.setChanceOfDF(0);
        else if (GameController.difficulty == 2) enemy.setChanceOfDF(0.35f);
        else if (GameController.difficulty == 3) enemy.setChanceOfDF(0.7f);
    }
    
    public void OnTick() {
        if (!inGame && endTick == -1) {
            endTick = GameController.currentTick+3;
            GameController.StopTimer();
        }
        
        if (GameController.currentTick == 5 && inGame) {
            endTick = 8;
            GameController.StopTimer();
            EndGame(true);
        }
        else if (GameController.currentTick == endTick) GameController.FinishGame(result);
    }

    public void EndGame(bool result) {
        inGame = false;
        this.result = result;
        
        if (result) winImg.GetComponent<Animation>().Play("ULC_TM_Win Animation");
        else loseImg.GetComponent<Animation>().Play("ULC_TM_Lose Animation");
        foreach (GameObject go in sceneObjects) go.SetActive(false);
        AudioManager.PlaySound(result ? victorySound : failureSound, 0.75f);
    }
}
