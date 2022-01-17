using System;
using UnityEditor;
using UnityEngine;
// ReSharper disable PossibleNullReferenceException

[CustomPropertyDrawer(typeof(GameTypeAttribute))]
// ReSharper disable once CheckNamespace
public class GameTypeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.propertyType != SerializedPropertyType.Integer)
        {
            EditorGUI.LabelField(position, label.text, "Use GameType with int.");
            return;
        }
        
        Type type = ((GameTypeAttribute) attribute).tClass;

        string[] choices = (string[]) type.GetMethod("GetTypeNames").Invoke(type, null);
        int placeholder = (int) type.GetMethod("RealValueAsDropdown").Invoke(type, new object[] {property.intValue});

        property.intValue = EditorGUI.Popup(position, "Type", placeholder, choices);
        property.intValue = (int) type.GetMethod("DropdownAsRealValue").Invoke(type, new object[] {property.intValue});
    }
}
