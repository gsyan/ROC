using UnityEngine;
using UnityEditor;

namespace com.sbp.ai
{
    [CustomPropertyDrawer(typeof(AirBalloonStep))]
    public class AniStepsBKDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            //base.OnGUI(position, property, label);

            EditorGUI.BeginProperty(position, label, property);

            // Draw label
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            // Don't make child fields be indented
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            // Calculate rects
            var stepNameRect = new Rect(position.x, position.y, 80, position.height);
            var stepValueRect = new Rect(position.x + 83, position.y, 70, position.height);

            // Draw fields - passs GUIContent.none to each so they are drawn without labels
            EditorGUI.PropertyField(stepNameRect, property.FindPropertyRelative("stepName"), GUIContent.none);
            EditorGUI.PropertyField(stepValueRect, property.FindPropertyRelative("stepValue"), GUIContent.none);


            // Set indent back to what it was
            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }

    }
}