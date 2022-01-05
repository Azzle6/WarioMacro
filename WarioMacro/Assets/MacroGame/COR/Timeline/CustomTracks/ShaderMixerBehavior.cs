using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class ShaderMixerBehavior : PlayableBehaviour
{
    public string shaderVarName;

    public override void ProcessFrame(
    Playable playable, FrameData info, object playerData)
    {
        Renderer rend = playerData as Renderer;
        if (rend == null)
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
            mat = rend.material;
        else
            mat = rend.sharedMaterial;
        mat.SetFloat(shaderVarName, finalFloat);
    }
}
