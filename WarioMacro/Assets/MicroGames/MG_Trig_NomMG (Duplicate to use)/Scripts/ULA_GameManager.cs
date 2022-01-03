using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ULA_GameManager : MonoBehaviour, ITickable
{
    public static ULA_GameManager instance;
    [SerializeField] private List<GameObject> levels;
    public AudioClip[] audioClips;
    private bool result;
    public bool init;
    private GameObject prefabs;
    private int lvl;
    private void Awake()
    {
        instance = this;
        init = true;
        lvl = Random.Range(0, 3);
        prefabs = Instantiate(levels[lvl]);
        prefabs.transform.position = Vector3.zero;
    }

    private void Start()
    {
        GameManager.Register(); //Mise en place du Input Manager, du Sound Manager et du Game Controller
        GameController.Init(this); //Permet a ce script d'utiliser le OnTick
        
        StartCoroutine(CoroutineFinInit());
        
        AudioManager.PlaySound(audioClips[1]);
        result = true;
    }

    public void OnTick()
    {
            ULA_CameraShake.instance.Shake(0.5f);
            Debug.Log("Tick");
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

    IEnumerator CoroutineFinInit()
    {
        yield return new WaitForSeconds(1.25f);
        AudioManager.PlaySound(audioClips[0],0.2f);
        AudioManager.PlaySound(audioClips[2]);
        init = false;
    }

    public void SetResult(bool b)
    {
        result = b;
    }
}
