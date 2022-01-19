using UnityEngine;

public class GameSettingsManager : MonoBehaviour
{
    public GameControllerSO gameControllerSO;
    [SerializeField] private BPMSettingsSO bpmSettingsSO;

    public void IncreaseDifficulty()
    {
        AudioManager.MacroPlaySound("NodeSuccess", 0);
        gameControllerSO.currentDifficulty++;
        ClampDifficulty();
    }
    
    public void DecreaseDifficulty()
    {
        AudioManager.MacroPlaySound("NodeFailure", 0);
        gameControllerSO.currentDifficulty--;
        ClampDifficulty();
    }

    public void IncreaseBPM()
    {
        AudioManager.MacroPlaySound("SpeedUp", 0);
        gameControllerSO.currentGameSpeed += bpmSettingsSO.increasingBPM;
        ClampBPM();
    }
    
    public void DecreaseBPM()
    {
        AudioManager.MacroPlaySound("SpeedDown", 0);
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
