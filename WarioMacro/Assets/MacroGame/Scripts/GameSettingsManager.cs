using UnityEngine;

public class GameSettingsManager : MonoBehaviour
{
    public GameControllerSO gameControllerSO;
    [SerializeField] private BPMSettingsSO bpmSettingsSO;

    public void IncreaseDifficulty()
    {
        gameControllerSO.currentDifficulty++;
        ClampDifficulty();
    }
    
    public void DecreaseDifficulty()
    {
        gameControllerSO.currentDifficulty--;
        ClampDifficulty();
    }

    public void IncreaseBPM()
    {
        gameControllerSO.currentGameSpeed += bpmSettingsSO.increasingBPM;
        ClampBPM();
    }
    
    public void DecreaseBPM()
    {
        gameControllerSO.currentGameSpeed -= bpmSettingsSO.decreasingBPM;
        ClampBPM();
    }

    private void ClampDifficulty()
    {
        gameControllerSO.currentDifficulty = Mathf.Clamp(gameControllerSO.currentDifficulty, 1, 3);
    }

    private void ClampBPM()
    {
        gameControllerSO.currentGameSpeed = Mathf.Clamp(gameControllerSO.currentGameSpeed, bpmSettingsSO.minBPM, bpmSettingsSO.maxBPM);
    }
}
