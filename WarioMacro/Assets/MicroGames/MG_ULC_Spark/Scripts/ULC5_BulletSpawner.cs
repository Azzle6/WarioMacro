using System.Collections;
using UnityEngine;

public class ULC5_BulletSpawner : MonoBehaviour
{
    public static ULC5_BulletSpawner instance;
    
    [Header("Prefabs")]
    [SerializeField] private GameObject policeCarPrefab;
    [SerializeField] private GameObject multiBulletsPrefab;
    [SerializeField] private GameObject snipePrefab;
    
    [Header("Spawns")]
    [SerializeField] private Transform[] policeCarSpawns;
    [SerializeField] private Transform[] multiBulletsSpawns;

    [Header("Timers")]
    [SerializeField] private float timePoliceCars = 2;
    [SerializeField] private float timeMultiBullets = 2;
    [SerializeField] private float timeSnipe = 2;

    [Header("Activation")]
    [SerializeField] private bool policeCars;
    [SerializeField] private bool multiBullets;
    [SerializeField] private bool snipe;

    void Awake() {
        instance = this;
    }

    public void SetDifficulty(int difficulty) {
        if (difficulty == 1) {
            policeCars = true;
            timePoliceCars = 1.25f;
        }

        else if (difficulty == 2) {
            policeCars = true;
            timePoliceCars = 1.5f;
            multiBullets = true;
            timeMultiBullets = 1.5f;
        }

        else if (difficulty == 3) {
            policeCars = true;
            timePoliceCars = 2f;
            multiBullets = true;
            timeMultiBullets = 2f;
            snipe = true;
            timeSnipe = 2f;
        }
    }
    
    public void StartSpawn() {
        if (policeCars) StartCoroutine(PoliceCarCR());
        if (multiBullets) StartCoroutine(MultiBulletsCR());
        if (snipe) StartCoroutine(SnipeCR());
    }

    private IEnumerator PoliceCarCR() {
        yield return new WaitForSeconds(0.5f);
        while (ULC5_GameManager.instance.inGame) {
            Transform rdmSpawn = policeCarSpawns[0];
            Vector3 position = rdmSpawn.position + new Vector3(0, Random.Range(-3, 4), 0);
            Transform rdmSpawn2 = policeCarSpawns[1];
            Vector3 position2 = rdmSpawn2.position + new Vector3(0, Random.Range(-3, 4), 0);
            while (position.y == position2.y) position2 = rdmSpawn2.position + new Vector3(0, Random.Range(-3, 4), 0);
            
            Instantiate(policeCarPrefab, position, rdmSpawn.localRotation,transform);
            Instantiate(policeCarPrefab, position2, rdmSpawn2.localRotation,transform);
            yield return new WaitForSeconds(timePoliceCars);
        }
    }
    
    private IEnumerator MultiBulletsCR() {
        yield return new WaitForSeconds(0.5f);
        while (ULC5_GameManager.instance.inGame) {
            Instantiate(multiBulletsPrefab, multiBulletsSpawns[Random.Range(1, multiBulletsSpawns.Length)].position, Quaternion.identity,transform);
            yield return new WaitForSeconds(timeMultiBullets);
        }
    }
    
    

    private IEnumerator SnipeCR() {
        yield return new WaitForSeconds(0.5f);
        while (ULC5_GameManager.instance.inGame) {
            Instantiate(snipePrefab, transform.position, Quaternion.identity,transform);
            yield return new WaitForSeconds(timeSnipe);
        }
    }
}
