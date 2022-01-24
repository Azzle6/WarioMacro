using System;
using GameTypes;
using JetBrains.Annotations;
using UnityEngine;

[CreateAssetMenu(fileName = "Reward Chart", menuName = "MacroGame/Reward Chart", order = 0)]
public class RewardChart : ScriptableObject
{
    [SerializeField] private PhaseChart[] chart;

    public int GetMGNumber(int phase, NodeBehaviour nodeType)
    {
        return GetNodeChartField(phase, nodeType, "mgNumber");
    }
    
    public int GetMoneyBags(int phase, NodeBehaviour nodeType)
    {
        return GetNodeChartField(phase, nodeType, "moneyBags");
    }
    
    public int GetAlarmProgress(int phase, NodeBehaviour nodeType)
    {
        return GetNodeChartField(phase, nodeType, "alarmProgress");
    }

    private int GetNodeChartField(int phase, NodeBehaviour nodeType, string fieldName)
    {
        foreach (NodeChart nodeChart in chart[phase].nodeCharts)
        {
            if (nodeChart.nodeType == nodeType)
            {
                return (int) typeof(NodeChart).GetField(fieldName).GetValue(nodeChart);
            }
        }
        
        Debug.LogError($"No typed found for phase {phase} and type {nodeType}");
        return 0;
    }
    
    [Serializable]
    private class PhaseChart
    {
        public NodeChart[] nodeCharts;
    }
    
    [Serializable]
    private class NodeChart
    {
        public NodeBehaviour nodeType;
        [UsedImplicitly]
        public int mgNumber = 3;
        [UsedImplicitly]
        public int moneyBags = 10000;
        [UsedImplicitly]
        public int alarmProgress = 5;
    }
}
