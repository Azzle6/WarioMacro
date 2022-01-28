using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
using UnityEngine.UI;

public class NOC2_LaserManager : MonoBehaviour, ITickable
{
    public static NOC2_LaserManager instance;
    private int difficulty;
    
    [SerializeField] private NOC2_LaserControler[] ctrl;
    [SerializeField] private Camera camera;
    [SerializeField] private float positionOffSet;

    [SerializeField]
    private Text resultText, order;

    [SerializeField] 
    private GameObject flash;

    [SerializeField] private Animator backgroundAnim, policeAnim;
    [SerializeField] private Text hardText;

    private List<bool> win = new List<bool>();
    private bool result;
    [HideInInspector] public int currentTick;

    [SerializeField] private AudioClip[] sounds;
    public float[] soundDelay; 

    void Start()
    {
        if (NOC2_LaserManager.instance != null)
        {
            Destroy(this);
            return;
        }

        currentTick = 0;
        result = false;
        instance = this;
        GameManager.Register();
        GameController.Init(this);
        difficulty = GameController.difficulty;
        hardText.text = difficulty.ToString();
        LaserStart();
    }

    public void OnTick()
    {
        
        currentTick = GameController.currentTick;
        if (GameController.currentTick == 5)
        {
            GameController.StopTimer();
        }
        if (GameController.currentTick == 8)
        {
            GameController.FinishGame(result);
        }
    }

    //Initialisation des lasers au début du micro-jeu.
    private void LaserStart()
    {
        //Calculer la position des limites de l'écran.
        float screenHeight = camera.orthographicSize - positionOffSet;
        float screenWidth = camera.aspect * camera.orthographicSize - positionOffSet;
        
        Vector3[] laserPosition = new Vector3[]
        {
            new Vector3(0, -screenHeight, 0), 
            new Vector3(screenWidth, 0, 0), 
            new Vector3(0, screenHeight, 0),
            new Vector3(-screenWidth, 0, 0)
        };
        
        //Générer la position des lasers.
        int n = Random.Range(0, 4);
        int imax = 0;
        
        float angle = 90 * n;

        if (difficulty == 1) imax = 2;
        else if (difficulty == 2) imax = 3;
        else imax = ctrl.Length;

        for (int i = 0; i < imax; i++)
        {
            //Activer les lasers.
            ctrl[i].gameObject.SetActive(true);
            ctrl[i].cameraBounderies = laserPosition;
            
            //Les placer à différents endroits et attribuer une direction de déplacement.
            Vector3 linePosition = Vector2.zero;
            if (laserPosition[(n + i) % 4].x == 0) linePosition.x = Random.Range(-screenWidth, screenWidth);
            else linePosition.y = Random.Range(-screenHeight, screenHeight);
            ctrl[i].transform.localPosition = laserPosition[(n + i) % 4] + new Vector3(camera.transform.position.x, camera.transform.position.y, 0) + linePosition;
            ctrl[i].direction = linePosition.normalized * Mathf.Sign(Random.Range(-1f, 1f));
            ctrl[i].cameraBounderiesIndex = (n + i) % 4;
            if(difficulty != 1) ctrl[i].currentAngle = Random.Range(ctrl[i].minRot, ctrl[i].maxRot);
            ctrl[i].difficulty = difficulty - 1;
            
            //Les mettre à la bonne rotation.
            ctrl[i].startAngle = angle;
            //ctrl[i].transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            angle += 90;
        }
    }

    private IEnumerator DisplayResult(string result)
    {
        flash.gameObject.SendMessage("ScreenShaking", 1);
        flash.gameObject.SendMessage("Flash", 1);
        this.resultText.text = result;
        this.resultText.enabled = true;
        yield return new WaitUntil(() => GameController.currentTick == 8);
        this.resultText.enabled = false;
        win = new List<bool>();
    }

    public void AddResult(bool _result)
    {
        NOC2_Controller.instance.blocked = true;
        win.Add(_result);
        int i = 0;
        foreach (NOC2_LaserControler l in ctrl)
        {
            if (l.gameObject.active) i++;
        }
        if (win.Count == i)
        {
            foreach (bool b in win)
            {
                if (b)
                {
                    StartCoroutine(DisplayResult("DEFAITE"));
                    policeAnim.Play("Police");
                    result = false;
                    AudioManager.PlaySound(sounds[3], 0.3f);
                    return;
                }
            }
            StartCoroutine(DisplayResult("VICTOIRE"));
            result = true;
            AudioManager.PlaySound(sounds[4], 0.3f);
            backgroundAnim.Play("Win");
        }
    }

    public void PlaySound(int index)
    {
        AudioManager.PlaySound(sounds[index], 0.1f, 0);
    }
}
