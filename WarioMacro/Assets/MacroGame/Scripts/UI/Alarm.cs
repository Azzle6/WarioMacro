using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class Alarm : MonoBehaviour
{
    public static bool isActive { get; private set; }
    [SerializeField] private PlayableDirector director;
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
        if (isActive || !gameObject.activeSelf) return; // TODO : don't decrement on recruitment phase
        
        count -= (result ? increaseOnWin : increaseOnLose) * currentFactor;

        if (count <= 0)
        {
            count = 0;
            isActive = true;
            MusicManager.instance.state = Soundgroup.CurrentPhase.ESCAPE;
            Debug.Log("Alarm mode On");
        }
        
        director.Play();
        StartCoroutine(PauseAlarm((100 - count) * director.duration * 0.01d));
        
    }

    private IEnumerator PauseAlarm(double time)
    {
        yield return new WaitForSeconds((float) (time - director.initialTime));

        director.Pause();
        director.initialTime = time;
    }

    private void Update()
    {
        if (isActive) return;

        currentFactor = 1 + MapManager.floor * floorFactorIncrease
                          + (Ticker.gameBPM - 50) / 10 * bpmFactorIncrease 
                          + (Ticker.difficulty - 1) * difficultyFactorIncrease;
    }
}
