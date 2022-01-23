using GameTypes;
using UnityEngine;
using Random = UnityEngine.Random;

// ReSharper disable once CheckNamespace
public class RecruitmentNode : Node
{
    [GameType(typeof(NodeDomainType))]
    public int type;
}
