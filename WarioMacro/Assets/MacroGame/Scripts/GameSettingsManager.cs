using UnityEngine;
using UnityEngine.Serialization;

public class GameSettingsManager : MonoBehaviour
{
    public GameConfig gameConfig;
    [SerializeField] private BPMSettingsSO bpmSettingsSO;

    public void IncreaseDifficulty()
    {
        AudioManager.MacroPlaySound("NodeSuccess", 0);
        gameConfig.currentDifficulty++;
        ClampDifficulty();
    }
    
    public void DecreaseDifficulty()
    {
        AudioManager.MacroPlaySound("NodeFailure", 0);
        gameConfig.currentDifficulty--;
        ClampDifficulty();
    }

    public void IncreaseBPM()
    {
        AudioManager.MacroPlaySound("SpeedUp", 0);
        if (gameConfig.currentGameSpeed == bpmSettingsSO.maxBPM)
        {
            AudioManager.MacroPlayRandomSound("HighLimitVoices", 0);
        }
        else
        {
            gameConfig.currentGameSpeed =
                Mathf.Min(gameConfig.currentGameSpeed + bpmSettingsSO.increasingBPM, bpmSettingsSO.maxBPM);
            AudioManager.MacroPlayRandomSound("SpeedUpVoices", 0);
        }
    }
    
    public void DecreaseBPM()
    {
        AudioManager.MacroPlaySound("SpeedDown", 0);
        if (gameConfig.currentGameSpeed == bpmSettingsSO.minBPM)
        {
            AudioManager.MacroPlayRandomSound("LowLimitVoices", 0);
        }
        else
        {
            gameConfig.currentGameSpeed =
                Mathf.Max(gameConfig.currentGameSpeed - bpmSettingsSO.decreasingBPM, bpmSettingsSO.minBPM);
            AudioManager.MacroPlayRandomSound("SpeedDownVoices", 0);
        }
    }

    private void ClampDifficulty()
    {
        gameConfig.currentDifficulty = Mathf.Clamp(gameConfig.currentDifficulty, 1, 3);
    }
}
