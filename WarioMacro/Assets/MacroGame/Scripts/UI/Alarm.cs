using UnityEngine;

public class Alarm : MonoBehaviour
{
    [Range(0.1f, 15f)]
    [SerializeField] private float increaseOnWin = 2;
    [Range(0.1f, 15f)]
    [SerializeField] private float increaseOnLose = 5;
    [Range(0.01f, 10f)]
    [SerializeField] private float floorFactorIncrease;
    [Range(0.01f, 10f)]
    [SerializeField] private float bpmFactorIncrease;
    [Range(0.01f, 10f)]
    [SerializeField] private float difficultyFactorIncrease;

    private float count = 100;
    private float currentFactor = 1;

    public void DecrementCount(bool result)
    {
        count -= (result ? increaseOnWin : increaseOnLose) * currentFactor;
        Debug.Log(count);
    }

    private void Update()
    {
        currentFactor = 1 + MapManager.floor * floorFactorIncrease
                          + (Ticker.gameBPM - 100) / 10 * bpmFactorIncrease 
                          + (Ticker.difficulty - 1) * difficultyFactorIncrease;
    }
}
