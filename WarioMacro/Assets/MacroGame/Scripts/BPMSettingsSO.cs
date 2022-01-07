using UnityEngine;

[CreateAssetMenu(fileName = "BPMSettingsSO", menuName = "MacroGame/BPMSettingsSO", order = 0)]
public class BPMSettingsSO : ScriptableObject
{
    public int minBPM;
    public int maxBPM;
    public int increasingBPM;
    public int decreasingBPM;
}
