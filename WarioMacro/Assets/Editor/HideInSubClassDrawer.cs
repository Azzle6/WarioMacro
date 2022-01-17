using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
 
[CustomPropertyDrawer(typeof(HideInSubClassAttribute))]
// ReSharper disable once CheckNamespace
public class HideInSubClassAttributeDrawer : PropertyDrawer
{
 
    private bool ShouldShow(SerializedProperty property)
    {
        Type type = property.serializedObject.targetObject.GetType();
        FieldInfo field = type.GetField(property.name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        if (field == null)
            return false;
        Type declaringType = field.DeclaringType;
        return type == declaringType;
    }
 
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (ShouldShow(property))
            EditorGUI.PropertyField(position, property, new GUIContent(property.displayName));
    }
 
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if(ShouldShow(property))
            return base.GetPropertyHeight(property, label);
        
        return -2;
    }
}