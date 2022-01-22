using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ULC3_GameManager : MonoBehaviour, ITickable
{
    [SerializeField] [Range(1, 3)] private int difficulty = 1;

    [SerializeField] private GameObject[] safesPrefab;
    private GameObject safe; 

    private ULC3_Pivot currentPivot;
    private int pivotIndex = 0;

    private bool inGame = false;
    private bool result = false;
    private bool winGame = false;

    public Animator lockerAnimator;
    public Animator locker2Animator;
    public Animator wheelAnimator;

    public GameObject victory;
    public GameObject loose;

    private int endTick = -1;

    void Start()
    {
        //Mise en place du Input Manager, du Sound Manager et du Game Controller
        GameManager.Register();
        
        //Permet a ce script d'utiliser le OnTick
        GameController.Init(this);
        
        //Instantiate a safe according to the selected difficulty
        safe = Instantiate(safesPrefab[GameController.difficulty-1], new Vector3(0,1,0), Quaternion.identity, transform);
        
        //currentPivot corresponds to which pivot we're currently moving
        currentPivot = safe.transform.GetChild(pivotIndex).GetComponent<ULC3_Pivot>();
        currentPivot.isSelected = true;

        inGame = true;
    }

    void Update()
    {
        if (inGame)
        {
            if (InputManager.GetKeyDown(ControllerKey.A))
            {
                //If currentPos matches the unlock position of the pivot
                if (currentPivot.currentPos == currentPivot.unlockPos)
                {
                    pivotIndex++;

                    //If current pivot is the last pivot of the safe
                    if (pivotIndex == safe.transform.childCount)
                    {
                        ULC3_AudioManager.instance.PlaySFX("Unlock", 1);
                        WinGame();
                        return;
                    }
                    
                    //Switch to next pivot
                    currentPivot.isSelected = false;
                    currentPivot = safe.transform.GetChild(pivotIndex).GetComponent<ULC3_Pivot>();
                    currentPivot.isSelected = true;
                }
                else
                {
                    ULC3_AudioManager.instance.PlaySFX("Select", 1);
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (inGame)
        {
            //Get input value
            currentPivot.lastStickInput = currentPivot.leftStickInput;
            currentPivot.leftStickInput = new Vector2(InputManager.GetAxis(ControllerAxis.LEFT_STICK_VERTICAL), InputManager.GetAxis(ControllerAxis.LEFT_STICK_HORIZONTAL));
        }
    }
    
    public void OnTick()
    {
        if (!inGame && endTick == -1)
        {
            GameController.StopTimer();
            endTick = GameController.currentTick + 3;
        }
        
        if (GameController.currentTick == 5 && inGame)
        {
            LooseGame();
            GameController.StopTimer();
            endTick = 8;
        }

        if (GameController.currentTick == endTick)
        {
            GameController.FinishGame(result);
        }
    }

    public void WinGame()
    {
        winGame = true;
        currentPivot.isSelected = false;
        
        // Lance la coroutine de victoire
        StartCoroutine("SafeOpen");
        
        inGame = false;
        result = true;
    }

    public void LooseGame()
    {
        inGame = false;
        currentPivot.isSelected = false;
        // Lance le feedback de défaite
        if (winGame == true)
        {
            loose.SetActive(false);
        }
        else if (winGame == false)
        {
            loose.SetActive(true);
            ULC3_AudioManager.instance.PlaySFX("Alarm", 1);
        }
    }

    IEnumerator SafeOpen()
    {
        yield return new WaitForSeconds(0.1f);
        lockerAnimator.SetTrigger("OpenLocker1");
        locker2Animator.SetTrigger("OpenLocker2");
        ULC3_AudioManager.instance.PlaySFX("OpenLocker", 1);
        yield return new WaitForSeconds(0.5f);
        wheelAnimator.SetTrigger("OpenWheel");
        ULC3_AudioManager.instance.PlaySFX("OpenWheel", 1);
        yield return new WaitForSeconds(0.5f);
        victory.SetActive(true);
        ULC3_AudioManager.instance.PlaySFX("Victory", 1);
        yield return new WaitForSeconds(1.9f);
        GameController.FinishGame(result);
    }
}
