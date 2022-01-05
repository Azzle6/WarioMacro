using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ULC_TM_GameManager : MonoBehaviour, ITickable
{
    public static ULC_TM_GameManager instance;
    
    //[SerializeField] private TMP_Text tickText;
    [SerializeField] private Image winImg, loseImg;
    [SerializeField] private List<GameObject> sceneObjects;

    [SerializeField] private ULC_TM_Enemy enemy;

    private bool inGame;
    private bool result;
    
    void Awake()
    {
        instance = this;
        inGame = true;
    }
    
    void Start()
    {
        GameManager.Register();
        GameController.Init(this);
        result = true;

        if (GameController.difficulty == 1) {
            enemy.setChanceOfDF(0);
        }
        else if (GameController.difficulty == 2) {
            enemy.setChanceOfDF(1);
        }
        else if (GameController.difficulty == 3) {
            enemy.setChanceOfDF(0.5f);
        }
        
    }
    
    public void OnTick()
    {
        //tickText.text = ""+(8-GameController.currentTick);
        if (GameController.currentTick == 4) enemy.stopFire(); 
        if (GameController.currentTick == 5 && inGame) EndGame();
        if (GameController.currentTick == 8) GameController.FinishGame(result);
    }
    
    public void LoseGame()
    {
        result = false;
        loseImg.GetComponent<Animation>().Play("Lose");
        EndGame();
    }

    public void EndGame()
    {
        inGame = false;
        if (result) winImg.GetComponent<Animation>().Play("Win");
        foreach (GameObject go in sceneObjects) go.SetActive(false);
    }

    public bool isInGame() {return this.inGame;}
}
