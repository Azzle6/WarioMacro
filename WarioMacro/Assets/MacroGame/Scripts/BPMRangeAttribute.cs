using UnityEngine;

// ReSharper disable once CheckNamespace
public class BPMRangeAttribute : PropertyAttribute
{
    public readonly int min;
    public readonly int max;
    public readonly int step;
    public readonly int numberOfSteps;

    public BPMRangeAttribute()
    {
        
        
    }
}
