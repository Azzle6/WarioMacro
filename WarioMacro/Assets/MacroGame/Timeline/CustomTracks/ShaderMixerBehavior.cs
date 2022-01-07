using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class ShaderMixerBehavior : PlayableBehaviour
{
    public string shaderVarName;

    public override void ProcessFrame(
    Playable playable, FrameData info, object playerData)
    {
        Image img = playerData as Image;
        if (img == null)
            return;
        int inputCount = playable.GetInputCount();
        float finalFloat = 0;
        for (int index = 0; index < inputCount; index++)
        {
            float weight = playable.GetInputWeight(index);
            var inputPlayable = (ScriptPlayable<ShaderControlBehavior>)playable.GetInput(index);
            ShaderControlBehavior behavior = inputPlayable.GetBehaviour();
            finalFloat += behavior.floatValue * weight;
        }
        Material mat;
        if (Application.isPlaying)
            mat = img.material;
        else
            mat = img.material;
        mat.SetFloat(shaderVarName, finalFloat);
    }
}
