using System;
using GameTypes;
using UnityEngine;

// ReSharper disable once CheckNamespace
public class GameTypeAttribute : PropertyAttribute
{
    public Type tClass;
    public int n = 0;

    public GameTypeAttribute(Type tClass)
    {
        this.tClass = tClass;
    }
}
