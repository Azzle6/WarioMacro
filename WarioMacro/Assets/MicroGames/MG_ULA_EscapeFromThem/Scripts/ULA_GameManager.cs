using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class ULA_GameManager : MonoBehaviour, ITickable
{
    public static ULA_GameManager instance;
    public AudioClip[] audioClips;

    [SerializeField] private Animator playerAnim;
    [SerializeField] private Animator ennemiAnim;
    [SerializeField] private Animator ennemi2Anim;
    
    
    [SerializeField] private List<GameObject> levelsEasy;
    [SerializeField] private List<GameObject> levelsMedium;
    [SerializeField] private List<GameObject> levelsHard;

    [SerializeField] private Transform lvlParent;
    public bool cinematic;

    private bool result;

    
    private GameObject prefabs;
    private int lvl;
    private void Awake()
    {
        instance = this;
        cinematic = true;
    }

    private void Start()
    {
        result = true;
        cinematic = true;
        GameManager.Register(); //Mise en place du Input Manager, du Sound Manager et du Game Controller
        GameController.Init(this); //Permet a ce script d'utiliser le OnTick
            
        switch (GameController.difficulty)
        {
            case 1:
                prefabs = Instantiate(levelsEasy[lvl],lvlParent);
                prefabs.transform.position = Vector3.zero;
                break;
            case 2:
                prefabs = Instantiate(levelsMedium[lvl],lvlParent);
                prefabs.transform.position = Vector3.zero;
                break;
            case 3 :
                prefabs = Instantiate(levelsHard[lvl],lvlParent);
                prefabs.transform.position = Vector3.zero;
                break;
                
        }
        
        StartCoroutine(CoroutineFinInit());
        AudioManager.PlaySound(audioClips[1]);
    }

    public void OnTick()
    {
            ULA_CameraShake.instance.Shake(0.5f);

            if (GameController.currentTick == 5)
            {
                //Le jeu se finit, il nous reste 3 ticks pour afficher le résultat
            }

            if (GameController.currentTick == 8)
            {
                Debug.Log(result);
                GameController.FinishGame(result);
            }
    }

    public void Win()
    {
        result = true;
        cinematic = true;
        AudioManager.PlaySound(audioClips[3]);
        ennemiAnim.SetBool("Win", true);
        ennemi2Anim.SetBool("Win", true);
    }

    public void Loose()
    {
        result = false;
        cinematic = true;
        playerAnim.SetBool("Loose", true);
        ennemiAnim.SetBool("Loose", true);
        ennemi2Anim.SetBool("Loose", true);
        AudioManager.StopSound(audioClips[2]);
    }

    IEnumerator CoroutineFinInit()
    {
        yield return new WaitForSeconds(1.25f);
        AudioManager.PlaySound(audioClips[0],0.2f);
        AudioManager.PlaySound(audioClips[2]);
        cinematic = false;
    }

    public void SetResult(bool b)
    {
        result = b;
    }
}
