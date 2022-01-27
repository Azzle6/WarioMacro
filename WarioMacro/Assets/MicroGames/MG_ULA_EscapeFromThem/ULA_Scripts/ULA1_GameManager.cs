using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class ULA1_GameManager : MonoBehaviour, ITickable
{
    public static ULA1_GameManager instance;
    public AudioClip[] audioClips;

    [SerializeField] private Animator playerAnim;
    [SerializeField] private Animator ennemiAnim;
    [SerializeField] private Animator ennemi2Anim;
    
    
    [SerializeField] private List<GameObject> levelsEasy;
    [SerializeField] private List<GameObject> levelsMedium;
    [SerializeField] private List<GameObject> levelsHard;

    [SerializeField] private TMP_Text victory; 
    [SerializeField] private TMP_Text defeat; 
    
    [SerializeField] private Transform lvlParent;
    public bool cinematic;

    private bool result;
    private int tickLoose;

    private bool stop;
    private GameObject prefabs;
    private int lvl;
    private void Awake()
    {
        instance = this;
        cinematic = true;
    }

    private void Start()
    {
        lvl = Random.Range(0, 3);
        result = true;
        cinematic = true;
        GameManager.Register(); //Mise en place du Input Manager, du Sound Manager et du Game Controller
        GameController.Init(this); //Permet a ce script d'utiliser le OnTick
            
        switch (GameController.difficulty)
        {
            case 1:
                prefabs = Instantiate(levelsEasy[lvl],lvlParent);
                prefabs.transform.position = new Vector3(0,0,-7f);
                break;
            case 2:
                prefabs = Instantiate(levelsMedium[lvl],lvlParent);
                prefabs.transform.position = new Vector3(0,0,-7f);
                break;
            case 3 :
                prefabs = Instantiate(levelsHard[lvl],lvlParent);
                prefabs.transform.position = new Vector3(0,0,-7f);
                break;
                
        }
        
        StartCoroutine(CoroutineFinInit());
        AudioManager.PlaySound(audioClips[1]);
    }

    public void OnTick()
    {
            ULA1_CameraShake.instance.Shake(0.5f);
            Debug.Log(GameController.currentTick);
            
            if (cinematic && GameController.currentTick>=2 && !stop)
            {
                stop = true;
                Debug.Log(GameController.currentTick);
                GameController.StopTimer();

            }
            
            if (GameController.currentTick == 5 && !stop)
            {
                stop = true;
                Loose();
                GameController.StopTimer();
                tickLoose = GameController.currentTick;
            }

            if (tickLoose!=0 && GameController.currentTick == tickLoose + 3)
            {
                Debug.Log(result);
                GameController.FinishGame(result);
            }
    }

    public void Win()
    {
        tickLoose = GameController.currentTick;
        Debug.Log(tickLoose);
        victory.enabled = true;
        result = true;
        cinematic = true;
        AudioManager.PlaySound(audioClips[3]);
        AudioManager.PlaySound(audioClips[4]);
        ennemiAnim.SetBool("Win", true);
        ennemi2Anim.SetBool("Win", true);
    }

    public void Loose()
    {
        tickLoose = GameController.currentTick;
        Debug.Log(tickLoose);
        defeat.enabled = true;
        result = false;
        cinematic = true;
        AudioManager.PlaySound(audioClips[5]);
        playerAnim.SetBool("Loose", true);
        ennemiAnim.SetBool("Loose", true);
        ennemi2Anim.SetBool("Loose", true);
        AudioManager.StopSound(audioClips[2]);
    }

    IEnumerator CoroutineFinInit()
    {
        yield return new WaitForSeconds(1.25f);
        AudioManager.PlaySound(audioClips[2]);
        cinematic = false;
    }

    public void SetResult(bool b)
    {
        result = b;
    }
}
