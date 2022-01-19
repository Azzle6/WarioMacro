using UnityEngine;
using UnityEngine.UI;

public class ULC5_GameManager : MonoBehaviour, ITickable
{
    public static ULC5_GameManager instance;
    public bool inGame;
    private bool result;

    [SerializeField] private GameObject[] sceneObjects;
    [SerializeField] private Image winImage, loseImage;

    [SerializeField] private AudioClip victorySound, failureSound;

    private int endTick = 10;
    
    void Awake() {
        instance = this;
        inGame = true;
    }

    void Start() {
        GameManager.Register();
        GameController.Init(this);
        result = true;
        
        ULC5_BulletSpawner.instance.SetDifficulty(GameController.difficulty);
        ULC5_BulletSpawner.instance.StartSpawn();
    }
    
    public void OnTick() {
        if (GameController.currentTick == 5 && inGame) {
            GameController.StopTimer();
            EndGame(true);
        }
        else if (GameController.currentTick == endTick+3) GameController.FinishGame(result);
    }

    public void EndGame(bool result) {
        inGame = false;
        this.result = result;
        endTick = GameController.currentTick;
        
        foreach (GameObject go in sceneObjects) go.SetActive(false);
        if (result) winImage.gameObject.SetActive(true);
        else loseImage.gameObject.SetActive(true);
        AudioManager.PlaySound(result ? victorySound : failureSound, 0.5f);
    }
}
