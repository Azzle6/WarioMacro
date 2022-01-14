using UnityEngine;

// ReSharper disable once CheckNamespace
public class NodeSettings : MonoBehaviour
{
    public int microGamesNumber;
    public Type type;
    
    // TODO : synchronise with player types
    public enum Type 
    {
        None,
        Brute,
        Alchemist,
        Expert,
        Ghost,
        Acrobat,
        Technomancer
    }
}
