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
        if (gameControllerSO.currentGameSpeed == bpmSettingsSO.maxBPM)
        {
            AudioManager.MacroPlayRandomSound("HighLimitVoices", 0);
        }
        else
        {
            gameControllerSO.currentGameSpeed =
                Mathf.Min(gameControllerSO.currentGameSpeed + bpmSettingsSO.increasingBPM, bpmSettingsSO.maxBPM);
            AudioManager.MacroPlayRandomSound("SpeedUpVoices", 0);
        }
    }
    
    public void DecreaseBPM()
    {
        AudioManager.MacroPlaySound("SpeedDown", 0);
        if (gameControllerSO.currentGameSpeed == bpmSettingsSO.maxBPM)
        {
            AudioManager.MacroPlayRandomSound("LowLimitVoices", 0);
        }
        else
        {
            gameControllerSO.currentGameSpeed =
                Mathf.Max(gameControllerSO.currentGameSpeed - bpmSettingsSO.decreasingBPM, bpmSettingsSO.minBPM);
            AudioManager.MacroPlayRandomSound("SpeedDownVoices", 0);
        }
    }

    private void ClampDifficulty()
    {
        gameControllerSO.currentDifficulty = Mathf.Clamp(gameControllerSO.currentDifficulty, 1, 3);
    }
}
