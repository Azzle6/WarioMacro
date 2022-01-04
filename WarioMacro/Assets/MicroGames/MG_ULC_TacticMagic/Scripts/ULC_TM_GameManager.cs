using TMPro;
using UnityEngine;

public class ULC_TM_GameManager : MonoBehaviour, ITickable
{
    public static ULC_TM_GameManager instance;
    [SerializeField] private ULC_TM_Enemy enemy;
    [SerializeField] private TMP_Text tickText;
    
    private Animator playerAnimator;
    private Animator enemyAnimator;

    public bool isRunning;
    public bool result;
    
    void Awake()
    {
        instance = this;
        isRunning = true;
    }
    
    void Start()
    {
        GameManager.Register();
        GameController.Init(this);

        enemy.chanceOfDoubleFire = (GameController.difficulty-1) / 3f;
        
        playerAnimator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        enemyAnimator = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Animator>();
        
        enemyAnimator.SetTrigger("Hurt");
    }
    
    public void OnTick()
    {
        if (isRunning) tickText.text = ""+(8-GameController.currentTick);
        if (GameController.currentTick == 8)
        {
            GameController.FinishGame(result);
        }
    }

    public void LoseGame()
    {
        result = false;
        isRunning = false;
        tickText.text = "Lose";
    }
}
