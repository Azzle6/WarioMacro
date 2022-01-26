using System.Collections;
using GameTypes;
using UnityEngine;
using UnityEngine.UI;

public class Alarm : MonoBehaviour
{
    public static bool isActive { get; private set; }
    [SerializeField] private RewardChart rewardChart;
    [SerializeField] private GameObject actionPostProcess;
    [SerializeField] private GameObject alarmPostProcess;
    [SerializeField] private Image gauge;
    [SerializeField] private float speed;

    private float count = 100;

    public void DecrementCount(int phase, NodeBehaviour nodeBehaviour)
    {
        // Don't decrement on recruitment phase
        if (isActive || !gameObject.activeSelf) return;

        count -= rewardChart.GetAlarmProgress(phase, nodeBehaviour);

        if (count <= 0)
        {
            Activate();
        }
        
        StartCoroutine(DecreaseAlarm((100 - count) * 0.01f));
        
    }

    public void FillAllAlarm()
    {
        gauge.fillAmount = 100;
        count = 0;
        Activate();
    }

    private IEnumerator DecreaseAlarm(float stop)
    {
        while (gauge.fillAmount < stop)
        {
            gauge.fillAmount += speed * 0.001f;
            yield return null;
        }
    }

    private void Activate()
    {
        count = 0;
        isActive = true;
        actionPostProcess.SetActive(false);
        GameController.instance.macroObjects.Remove(actionPostProcess);
        alarmPostProcess.SetActive(true);
        GameController.instance.macroObjects.Add(alarmPostProcess);
        MusicManager.instance.state = Soundgroup.CurrentPhase.ESCAPE;
        AudioManager.MacroPlaySound("Alarm", 0);
        GameController.instance.stopLoop = true;
    }
}
