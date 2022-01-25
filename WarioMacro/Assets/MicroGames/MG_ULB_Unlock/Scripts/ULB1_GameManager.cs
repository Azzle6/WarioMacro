using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class ULB1_GameManager : MonoBehaviour, ITickable
{
    [SerializeField] private Camera cam;
    [SerializeField] private string[] easyLevels;
    [SerializeField] private string[] middleLevels;
    [SerializeField] private string[] hardLevels;
    private int randomMaze;
    private string chosenMaze;
    [SerializeField] private Transform player;
    private GameObject maze;
    [SerializeField] private Animation cameraAnim;
    [SerializeField] private Animation chestAnim;
    [SerializeField] private GameObject chest;
    [SerializeField] private AnimationClip cameraLevel1;
    [SerializeField] private AnimationClip cameraLevel2;
    [SerializeField] private AnimationClip cameraLevel3;
    private bool isPlay;
    private float gradientTime;
    [SerializeField] private Animation unlockAnim;
    [SerializeField] private MeshRenderer buttonMat;
    [SerializeField] private Material green;
    [SerializeField] private AudioClip footStep;
    [SerializeField] private AudioClip handCuffSound;
    [SerializeField] private ULB1_PlayerMovement playerMovement;
    public static ULB1_GameManager instance;

    [SerializeField] private GameObject handcuff;
    [SerializeField] private GameObject paint;
    [SerializeField] private GameObject arrow;
    [SerializeField] private Material redArrow;
    [SerializeField] private Material redArrowHDR;

    [SerializeField] private bool isWin;

    [SerializeField] private bool isFinish;

    [SerializeField] private int finishTick;

    [SerializeField] private bool isStopTimer;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        GameManager.Register();
        GameController.Init(this);
        Init();
    }

    void Init()
    {
        switch (GameController.difficulty)
        {
            case 1 :
                StartCoroutine("Level1");
                break;
            case 2 :
                StartCoroutine("Level2");
                break;
            case 3 :
                StartCoroutine("Level3");
                break;
            default:
                Debug.Log("Incorrect level");
                break;
        }
    }

    IEnumerator Level1()
    {
        cameraAnim.clip = cameraLevel1;
        cameraAnim.Play();
        yield return new WaitForSeconds(0.3f);
        randomMaze = Random.Range(0, easyLevels.Length);
        chosenMaze = easyLevels[randomMaze];
        maze = ULB1_Pooler.instance.Pop(chosenMaze);
        maze.transform.position = new Vector3(0,-401,0);
        player.position = maze.transform.GetChild(maze.transform.childCount - 1).transform.position;
        //cam.orthographicSize = 5;
        //cam.transform.position = new Vector3(0, 1, -10);
        chest.SetActive(false);
        isPlay = true;
        playerMovement.speed = 15;
    }
    
    IEnumerator Level2()
    {
        cameraAnim.clip = cameraLevel2;
        cameraAnim.Play();
        yield return new WaitForSeconds(0.3f);
        randomMaze = Random.Range(0, middleLevels.Length);
        chosenMaze = middleLevels[randomMaze];
        maze = ULB1_Pooler.instance.Pop(chosenMaze);
        maze.transform.position = new Vector3(0,-401,0);
        player.position = maze.transform.GetChild(maze.transform.childCount - 1).transform.position;
        //cam.orthographicSize = 7;
        //cam.transform.position = new Vector3(0, 1, -10);
        chest.SetActive(false);
        isPlay = true;
        playerMovement.speed = 17;
    }
    
    IEnumerator Level3()
    {
        cameraAnim.clip = cameraLevel3;
        cameraAnim.Play();
        yield return new WaitForSeconds(0.3f);
        randomMaze = Random.Range(0, hardLevels.Length);
        chosenMaze = hardLevels[randomMaze];
        maze = ULB1_Pooler.instance.Pop(chosenMaze);
        maze.transform.position = new Vector3(0,-401,0);
        player.position = maze.transform.GetChild(maze.transform.childCount - 1).transform.position;
        //cam.orthographicSize = 7;
        //cam.transform.position = new Vector3(0, 1, -10);
        chest.SetActive(false);
        isPlay = true;
        playerMovement.speed = 17;
    }

    public void FinishGame(bool Win)
    {
        player.gameObject.SetActive(false);
        if (GameController.currentTick!=5)
        {
            finishTick = GameController.currentTick+1;
        }
        else
        {
            finishTick = GameController.currentTick;
        }
        isFinish = true;
        if (Win && isPlay)
        {
            isWin = true;
            chest.SetActive(true);
            StartCoroutine(DePop());
            cameraAnim[cameraAnim.clip.name].time = 1;
            cameraAnim[cameraAnim.clip.name].speed = -1;
            cameraAnim.Play();
            unlockAnim.Play();
            buttonMat.material = green;
            StartCoroutine(OpenChest());
        }
        else
        {
            isWin = false;
            AudioManager.PlaySound(handCuffSound);
            cameraAnim[cameraAnim.clip.name].time = 1;
            cameraAnim[cameraAnim.clip.name].speed = -1;
            cameraAnim.Play();
            chest.SetActive(true);
            handcuff.SetActive(true);
            paint.SetActive(false);
            arrow.SetActive(true);
            StartCoroutine(ChangeArrowColor());
            StartCoroutine(DePop());
        }
    }

    IEnumerator OpenChest()
    {
        yield return new WaitForSeconds(0.5f);
        chestAnim.Play();
    }
    public void OnTick()
    {
        if (GameController.currentTick%2==0)
            {
                AudioManager.PlaySound(footStep);
            }

            if (GameController.currentTick == 5 && !isWin)
            {
                GameController.StopTimer();
                isStopTimer = true;
                if (!isFinish)
                {
                    FinishGame(false);
                }
            }

            if (GameController.currentTick == finishTick+3 && finishTick!=0)
            {
                GameController.FinishGame(isWin);
            }

            if (isFinish && !isStopTimer)
            {
                GameController.StopTimer();
                isStopTimer = true;
            }

    }
    

    IEnumerator ChangeArrowColor()
    {
        arrow.GetComponent<MeshRenderer>().material = redArrow;
        yield return new WaitForSeconds(0.5f);
        arrow.GetComponent<MeshRenderer>().material = redArrowHDR;
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(ChangeArrowColor());
    }

    IEnumerator DePop()
    {
        yield return new WaitForSeconds(0.5f);
        ULB1_Pooler.instance.DePop(chosenMaze, maze);
    }
}