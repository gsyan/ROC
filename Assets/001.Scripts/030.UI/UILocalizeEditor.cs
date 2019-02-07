using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CanEditMultipleObjects]
[CustomEditor(typeof(UILocalize), true)]
public class UILocalizeEditor : Editor
{
    private List<string> _keyList;


    private void OnEnable()
    {
        Dictionary<string, string[]> dict = Localization.dictionary;
        
        if ( dict.Count > 0)
        {
            _keyList = new List<string>();
            foreach(KeyValuePair<string, string[]> pair in dict)
            {
                if (pair.Key == "key") continue;
                _keyList.Add(pair.Key);
            }
            _keyList.Sort(delegate (string left, string right) { return left.CompareTo(right);});
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        GUILayout.Space(6f);

        
        GUILayout.BeginHorizontal();
        
        SerializedProperty sp = Utility.DrawProperty("Key", serializedObject, "key");
        
        //오늘의 할일

        


        GUILayout.EndHorizontal();

    }







    /// <summary>
    /// Convenience function that marks the specified object as dirty in the Unity Editor.
    /// </summary>
    public static void SetDirty(UnityEngine.Object obj)
    {
#if UNITY_EDITOR
        EditorUtility.SetDirty(obj);
#endif
    }

    //    public static SerializedProperty DrawProperty(string label, SerializedObject serializedObject, string property, bool padding, params GUILayoutOption[] options)
    //    {
    //        SerializedProperty sp = serializedObject.FindProperty(property);

    //        if (sp != null)
    //        {
    //            EditorGUILayout.BeginHorizontal();

    //            if (sp.isArray && sp.type != "string") DrawArray(serializedObject, property, label ?? property);
    //            else if (label != null) EditorGUILayout.PropertyField(sp, new GUIContent(label), options);
    //            else EditorGUILayout.PropertyField(sp, options);

    //            EditorGUILayout.EndHorizontal();

    //        }
    //        else Debug.LogWarning("Unalble to find property " + property);
    //        return sp;
    //    }
    //    public static void DrawArray(this SerializedObject obj, string property, string title)
    //    {
    //        SerializedProperty sp = obj.FindProperty(property + ".Array.size");

    //        if (sp != null)
    //        {
    //            int size = sp.intValue;
    //            int newSize = EditorGUILayout.IntField("Size", size);
    //            if (newSize != size) obj.FindProperty(property + ".Array.size").intValue = newSize;

    //            EditorGUI.indentLevel = 1;

    //            for (int i = 0; i < newSize; i++)
    //            {
    //                SerializedProperty p = obj.FindProperty(string.Format("{0}.Array.data[{1}]", property, i));
    //                if (p != null) EditorGUILayout.PropertyField(p);
    //            }
    //            EditorGUI.indentLevel = 0;
    //        }


    //    }

}
