using System;
using JetBrains.Annotations;
using UnityEngine;

public class Node : MonoBehaviour
{
    [CanBeNull] public Path[] paths;
    
    [Serializable]
    public class Path
    {
        [CanBeNull] public Node destination;
        [CanBeNull] public Transform[] steps;
    }
}
