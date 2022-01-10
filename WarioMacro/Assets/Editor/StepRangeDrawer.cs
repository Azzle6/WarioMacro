using System;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(BPMRangeAttribute))]
// ReSharper disable once CheckNamespace
public class StepRangeDrawer : PropertyDrawer
{
    // Draw the property inside the given rect
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.propertyType != SerializedPropertyType.Integer)
        {
            EditorGUI.LabelField(position, label.text, "Use Range with int.");
            return;
        }

        var bpmSettings = Resources.Load<BPMSettingsSO>("BPMSettingsSO");
        int min = bpmSettings.minBPM;
        int max = bpmSettings.maxBPM;
        int step = Mathf.Min(bpmSettings.decreasingBPM, bpmSettings.increasingBPM);
        int numberOfSteps = (max - min) / step;
        float range = ((float) property.intValue - min) / (max - min) * numberOfSteps;
        int ceil = Mathf.RoundToInt(range);
        property.intValue = ceil * step + min;
        
        EditorGUI.IntSlider(position, property, Convert.ToInt32(min), Convert.ToInt32(max), label);
        
    }
}
