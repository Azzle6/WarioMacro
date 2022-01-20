using UnityEngine;

public class NAB3_NMIGenerator : MonoBehaviour
{
    public GameObject enemy;

    public float MinSpeed;
    //public float MaxSpeed;
    public float currentSpeed;

    //public float SpeedMultiplier;

    public int difficulty;

    void Awake()
    {
        //difficulty = GameController.difficulty;

        currentSpeed = MinSpeed;
    }

    private void Start()
    {
        float randomWait = Random.Range(1.1f, 1.2f);
        Invoke("generateGuard", randomWait);
    }

    public void GenerateNextDogWithGap()
    {
        if (difficulty == 1)
        {
            float randomWait = Random.Range(5f, 5.1f);
            Invoke("generateGuard", randomWait);
        }
        if (difficulty == 2)
        {
            float randomWait = Random.Range(2.1f, 2.2f);
            Invoke("generateGuard", randomWait);
        }
        if (difficulty == 3)
        {
            float randomWait = Random.Range(0.8f, 1f);
            Invoke("generateGuard", randomWait);
        }
    }

    public void generateGuard()
    {
        GameObject NMIIns = Instantiate(enemy, transform.position, transform.rotation);

        NMIIns.GetComponent<NAB3_NMI>().enemyGenerator = this;
    }

    /*void Update()
    {
        if(currentSpeed < MaxSpeed)
        {
            currentSpeed += SpeedMultiplier;
        }
    }*/
}
