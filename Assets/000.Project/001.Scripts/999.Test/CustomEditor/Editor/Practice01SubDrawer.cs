using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(Practice01Sub))]
public class Practice01SubDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //base.OnGUI(position, property, label);

        EditorGUI.BeginProperty(position, label, property);

        // Draw label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // Don't make child fields be indented
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;//들여쓰기

        // Calculate rects
        var nameRect = new Rect(position.x, position.y, 80, position.height);
        var valueRect = new Rect(position.x + 83, position.y, 70, position.height);

        // Draw fields - passs GUIContent.none to each so they are drawn without labels
        EditorGUI.PropertyField(nameRect, property.FindPropertyRelative("name"), GUIContent.none);
        EditorGUI.PropertyField(valueRect, property.FindPropertyRelative("value"), GUIContent.none);


        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }


}
