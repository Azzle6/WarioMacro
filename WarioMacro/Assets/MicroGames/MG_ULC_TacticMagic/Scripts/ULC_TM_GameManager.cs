using UnityEngine;

public class ULC_TM_GameManager : MonoBehaviour, ITickable
{
    public static ULC_TM_GameManager instance;
    [SerializeField] private ULC_TM_Enemy enemy;
    
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

        enemy.chanceOfDoubleFire = GameController.difficulty / 3f;
        
        playerAnimator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        enemyAnimator = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Animator>();
        
        enemyAnimator.SetTrigger("Hurt");
    }
    
    public void OnTick()
    {
        Debug.Log(GameController.currentTick);
        if (GameController.currentTick == 8)
        {
            GameController.FinishGame(result);
        }
    }

    public void LoseGame()
    {
        result = false;
        isRunning = false;
        Debug.Log("Lose");
    }
}
