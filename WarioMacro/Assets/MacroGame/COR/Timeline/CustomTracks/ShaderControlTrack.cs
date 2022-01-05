using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;

[TrackColor(0.715f, 0.903f, 0.87f)]
[TrackClipType(typeof(ShaderControlClip))]
[TrackBindingType(typeof(Image))]
public class ShaderControlTrack : TrackAsset
{
    public string shaderVarName;
    public override Playable CreateTrackMixer(
        PlayableGraph graph, GameObject go, int inputCount)
    {
        var mixer = ScriptPlayable<ShaderMixerBehavior>.Create(graph, inputCount);
        mixer.GetBehaviour().shaderVarName = shaderVarName;
        return mixer;
    }
}
