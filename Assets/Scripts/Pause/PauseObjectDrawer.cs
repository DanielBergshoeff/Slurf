#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(PauseObject))]
public class PauseObjectDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        GUIContent behaviorName = new GUIContent(property.FindPropertyRelative("behavior").stringValue);

        var amountRect = new Rect(position.x, position.y, 30, position.height);
        EditorGUI.PropertyField(amountRect, property.FindPropertyRelative("pause"), behaviorName);

        EditorGUI.EndProperty();
    }
}
#endif