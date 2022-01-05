using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class ShaderControlClip : PlayableAsset
{
    public float floatValue = 0f;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<ShaderControlBehavior>.Create(graph);
        ShaderControlBehavior runtimePlayable = playable.GetBehaviour();
        runtimePlayable.floatValue = floatValue;
        return playable;
    }
}

