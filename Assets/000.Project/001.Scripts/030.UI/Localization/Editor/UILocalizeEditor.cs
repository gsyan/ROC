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


        SerializedProperty sp = GUIEditorToolsBK.DrawProperty("Key", serializedObject, "key");

        string key = sp.stringValue;
        bool isExist = (_keyList != null) && _keyList.Contains(key);//key 가 _keyList에 존재하는가
        GUI.color = isExist ? Color.green : Color.red;

        //v, x 표시
        GUILayout.BeginVertical(GUILayout.Width(22f));
        GUILayout.Space(2f);
        GUILayout.Label(isExist ? "\u2714" : "\u2718", "TL SelectionButtonNew", GUILayout.Height(20f));
        GUILayout.EndVertical();

        GUI.color = Color.white;
        GUILayout.EndHorizontal();

        //if (isExist)
        //{
        //    if(GUIEditorToolsBK.DrawHeader("preview"))
        //    {
        //        GUIEditorToolsBK.BeginContents();

        //        string language = "";
        //        string[] languageKey = Localization.knownLanguages;
        //        string[] values;

        //        if (Localization.dictionary.TryGetValue(key, out values))
        //        {
        //            if (languageKey.Length != values.Length)
        //            {
        //                EditorGUILayout.HelpBox("Number of keys doesn't match the number of values! Did you modify the dictionaries by hand at some point?", MessageType.Error);
        //            }
        //            else
        //            {
        //                for (int i = 0; i < languageKey.Length; ++i)
        //                {
        //                    GUILayout.BeginHorizontal();
        //                    GUILayout.Label(languageKey[i], GUILayout.Width(66f));

        //                    if (GUILayout.Button(values[i], "CN Box", GUILayout.MinWidth(80f), GUILayout.MaxWidth(Screen.width - 110f)))
        //                    {
        //                        (target as UILocalize).value = values[i];
        //                        GUIUtility.hotControl = 0;
        //                        GUIUtility.keyboardControl = 0;
        //                    }
        //                    GUILayout.EndHorizontal();
        //                }
        //            }
        //        }
        //        else
        //        {
        //            GUILayout.Label("No Preview Available");
        //        }

        //        GUIEditorToolsBK.EndContents();
        //    }//if(GUIEditorTools.DrawHeader("preview"))
        //}
        //else if( _keyList != null && !string.IsNullOrEmpty(key))
        //{
        //    GUILayout.BeginHorizontal();
        //    GUILayout.Space(80f);
        //    GUILayout.BeginVertical();
        //    GUI.backgroundColor = new Color(1f, 1f, 1f, 0.35f);

        //    int matches = 0;

        //    for(int i = 0, imax = _keyList.Count; i < imax; ++i)
        //    {
        //        if( _keyList[i].StartsWith(key, System.StringComparison.Ordinal) || _keyList[i].Contains(key))
        //        {
        //            if (GUILayout.Button(_keyList[i] + " \u25B2", "CN CountBadge"))
        //            {
        //                sp.stringValue = _keyList[i];
        //                GUIUtility.hotControl = 0;
        //                GUIUtility.keyboardControl = 0;
        //            }

        //            if (++matches == 8)
        //            {
        //                GUILayout.Label("...and more");
        //                break;
        //            }
        //        }
        //    }
        //    GUI.backgroundColor = Color.white;
        //    GUILayout.EndVertical();
        //    GUILayout.Space(22f);
        //    GUILayout.EndHorizontal();
        //}

        serializedObject.ApplyModifiedProperties();
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
