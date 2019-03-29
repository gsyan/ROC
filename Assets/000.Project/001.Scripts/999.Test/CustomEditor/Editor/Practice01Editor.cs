using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(Practice01))]
[CanEditMultipleObjects]
public class Practice01Editor : Editor
{
    Practice01 _script;

    SerializedProperty lookAtPosition;

    private void OnEnable()
    {
        _script = (Practice01)target;

        lookAtPosition = serializedObject.FindProperty("lookAtPosition");
    }


    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(lookAtPosition);

        EditorGUILayout.LabelField("(Above This Object)");

        _script.intValue = EditorGUILayout.IntField("Experence", _script.intValue);

        ListIterator("practiceArray", ref showChildren);

        serializedObject.ApplyModifiedProperties();
    }

    static bool showChildren = true;
    void ListIterator(string propertyName, ref bool visible)
    {
        SerializedProperty listProperty = serializedObject.FindProperty(propertyName);
        visible = EditorGUILayout.Foldout(visible, listProperty.name);
        if (visible)
        {
            EditorGUI.indentLevel++;

            EditorGUILayout.BeginHorizontal();
            Rect drawZoneLabel1 = GUILayoutUtility.GetRect(0, 16f);
            EditorGUI.LabelField(drawZoneLabel1, "name");
            Rect drawZoneLabel2 = GUILayoutUtility.GetRect(0, 16f);
            EditorGUI.LabelField(drawZoneLabel2, "value");
            Rect drawZoneLabel3 = GUILayoutUtility.GetRect(0, 16f);
            EditorGUI.LabelField(drawZoneLabel3, "");
            EditorGUILayout.EndHorizontal();

            for (int i = 0; i < listProperty.arraySize; i++)
            {
                SerializedProperty elementProperty = listProperty.GetArrayElementAtIndex(i);

                EditorGUILayout.BeginHorizontal();

                Rect drawZone1 = GUILayoutUtility.GetRect(0f, 16f);
                bool showChildren = EditorGUI.PropertyField(drawZone1, elementProperty, new GUIContent(""));

                Rect drawZone2 = GUILayoutUtility.GetRect(0, 16f);
                EditorGUI.LabelField(drawZone2, "");

                Rect drawZone3 = GUILayoutUtility.GetRect(0, 16f);
                if (GUI.Button(drawZone3, "button"))
                {
                    
                }

                EditorGUILayout.EndHorizontal();


                
            }


            ArrayLengthAddSubtractButton();

            EditorGUI.indentLevel--;
        }
    }
    void ArrayLengthAddSubtractButton()
    {
        EditorGUILayout.BeginHorizontal();

        Rect drawZoneLabel = GUILayoutUtility.GetRect(80f, 16f);
        EditorGUI.LabelField(drawZoneLabel, "Steps Length: " + _script.practiceArray.Length.ToString());


        Rect drawZone1 = GUILayoutUtility.GetRect(3f, 16f);
        if (GUI.Button(drawZone1, "+"))
        {
            if (_script.practiceArray != null)
            {
                int count = _script.practiceArray.Length;
                Array.Resize<Practice01Sub>(ref _script.practiceArray, count + 1);
                _script.practiceArray[count] = new Practice01Sub();
                _script.practiceArray[count].name = "element" + count.ToString();
                _script.practiceArray[count].value = 0.0f;
            }
            else
            {
                _script.practiceArray = new Practice01Sub[1];
                _script.practiceArray[0] = new Practice01Sub();
                _script.practiceArray[0].name = "element0";
                _script.practiceArray[0].value = 0.0f;
            }

        }

        Rect drawZone2 = GUILayoutUtility.GetRect(3f, 16f);
        if (GUI.Button(drawZone2, "-"))
        {
            if (_script.practiceArray != null)
            {
                int count = _script.practiceArray.Length;
                if (count > 0)
                {
                    Array.Resize<Practice01Sub>(ref _script.practiceArray, count - 1);
                }
            }
        }

        EditorGUILayout.EndHorizontal();
    }

}
