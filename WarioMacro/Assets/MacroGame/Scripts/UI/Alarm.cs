using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class Alarm : MonoBehaviour
{
    public static bool isActive { get; private set; }
    [SerializeField] private Image gauge;
    [SerializeField] private float speed;
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
        // Don't decrement on recruitment phase
        if (isActive || !gameObject.activeSelf) return; 
        
        count -= (result ? increaseOnWin : increaseOnLose) * currentFactor;

        if (count <= 0)
        {
            count = 0;
            isActive = true;
            MusicManager.instance.state = Soundgroup.CurrentPhase.ESCAPE;
            AudioManager.MacroPlaySound("Alarm", 0);
        }
        
        StartCoroutine(DecreaseAlarm((100 - count) * 0.01f));
        
    }

    private IEnumerator DecreaseAlarm(float stop)
    {
        while (gauge.fillAmount < stop)
        {
            gauge.fillAmount += speed * 0.001f;
            yield return null;
        }
    }

    private void Update()
    {
        if (isActive) return;

        currentFactor = 1 + MapManager.floor * floorFactorIncrease
                          + (Ticker.gameBPM - 50) / 10 * bpmFactorIncrease 
                          + (Ticker.difficulty - 1) * difficultyFactorIncrease;
    }
}
