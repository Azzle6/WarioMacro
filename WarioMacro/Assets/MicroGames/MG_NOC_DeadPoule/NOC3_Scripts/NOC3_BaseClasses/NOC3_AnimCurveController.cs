using UnityEngine;

public enum CurveType { Cubic, Parabolic, EaseOutElastic }
public static class NOC3_AnimCurveController
{
    public static float EaseOutElastic(float time)  
    {
        const float c4 = (float)(2f * Mathf.PI) / 3f;

        return time == 0 ? 0
        : time == 1 ? 1
        : Mathf.Pow(2, -10 * time) * Mathf.Sin((time * 10f - 0.75f) * c4) + 1;
    }
}
