using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ULB_Unlock_GameManager : MonoBehaviour, ITickable
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
    [SerializeField] private Gradient gradient;
    [SerializeField] private Image blackScreen;
    [SerializeField] private Image handCuff;
    [SerializeField] private TMP_Text failed;
    private float gradientTime;
    [SerializeField] private Animation unlockAnim;
    [SerializeField] private MeshRenderer buttonMat;
    [SerializeField] private Material green;
    [SerializeField] private AudioClip footStep;
    [SerializeField] private AudioClip handCuffSound;
    [SerializeField] private ULB_Unlock_PlayerMovement playerMovement;
    public static ULB_Unlock_GameManager instance;
    [SerializeField] private GameObject escape;

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
        yield return new WaitForSeconds(1);
        escape.SetActive(false);
        randomMaze = Random.Range(0, easyLevels.Length);
        chosenMaze = easyLevels[randomMaze];
        maze = ULB_Unlock_Pooler.instance.Pop(chosenMaze);
        maze.transform.position = new Vector3(0,-401,0);
        player.position = maze.transform.GetChild(maze.transform.childCount - 1).transform.position;
        cam.orthographicSize = 5;
        cam.transform.position = new Vector3(0, 1, -10);
        chest.SetActive(false);
        isPlay = true;
        playerMovement.speed = 15;
    }
    
    IEnumerator Level2()
    {
        cameraAnim.clip = cameraLevel2;
        cameraAnim.Play();
        yield return new WaitForSeconds(1);
        escape.SetActive(false);
        randomMaze = Random.Range(0, middleLevels.Length);
        chosenMaze = middleLevels[randomMaze];
        maze = ULB_Unlock_Pooler.instance.Pop(chosenMaze);
        maze.transform.position = new Vector3(0,-401,0);
        player.position = maze.transform.GetChild(maze.transform.childCount - 1).transform.position;
        cam.orthographicSize = 7;
        cam.transform.position = new Vector3(0, 1, -10);
        chest.SetActive(false);
        isPlay = true;
        playerMovement.speed = 17;
    }
    
    IEnumerator Level3()
    {
        cameraAnim.clip = cameraLevel3;
        cameraAnim.Play();
        yield return new WaitForSeconds(1);
        escape.SetActive(false);
        randomMaze = Random.Range(0, hardLevels.Length);
        chosenMaze = hardLevels[randomMaze];
        maze = ULB_Unlock_Pooler.instance.Pop(chosenMaze);
        maze.transform.position = new Vector3(0,-401,0);
        player.position = maze.transform.GetChild(maze.transform.childCount - 1).transform.position;
        cam.orthographicSize = 7;
        cam.transform.position = new Vector3(0, 1, -10);
        chest.SetActive(false);
        isPlay = true;
        playerMovement.speed = 17;
    }

    public void FinishGame(bool Win)
    {
        if (Win && isPlay)
        {
            chest.SetActive(true);
            ULB_Unlock_Pooler.instance.DePop(chosenMaze,maze);
            cameraAnim[cameraAnim.clip.name].time = 1;
            cameraAnim[cameraAnim.clip.name].speed = -1;
            cameraAnim.Play();
            unlockAnim.Play();
            buttonMat.material = green;
            StartCoroutine(OpenChest());
            isPlay = false;
        }
        else
        {
            AudioManager.PlaySound(handCuffSound);
            StartCoroutine(BlackScreen());
        }
    }

    IEnumerator OpenChest()
    {
        yield return new WaitForSeconds(0.5f);
        chestAnim.Play();
        yield return new WaitForSeconds(2f);
        GameController.FinishGame(true);
    }
    public void OnTick()
    {
        if (isPlay)
        {
            if (GameController.currentTick%2==0)
            {
                AudioManager.PlaySound(footStep);
            }

            if (GameController.currentTick == 8)
            {
                StartCoroutine(BlackScreen());
            }
        }
    }

    IEnumerator BlackScreen()
    {
        isPlay = false;
        gradientTime += 0.01f;
        blackScreen.color = blackScreen.color + new Color(0,0,0,gradient.Evaluate(gradientTime).a);
        handCuff.color = handCuff.color + new Color(0,0,0,gradient.Evaluate(gradientTime).a);
        failed.color = failed.color + new Color(0,0,0,gradient.Evaluate(gradientTime).a);
        yield return new WaitForSeconds(0.1f);
        if (gradientTime<1)
        {
            StartCoroutine(BlackScreen());
        }

        yield return new WaitForSeconds(2);
        GameController.FinishGame(false);
    }
}