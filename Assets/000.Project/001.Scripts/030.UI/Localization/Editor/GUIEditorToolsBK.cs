using UnityEngine;
using UnityEditor;

public class GUIEditorToolsBK
{
    public static SerializedProperty DrawProperty(string editorName, SerializedObject serializedObject, string propertyName, params GUILayoutOption[] options)
    {
        return DrawProperty(editorName, serializedObject, propertyName, false, options);
    }
    public static SerializedProperty DrawProperty(string editorName, SerializedObject serializedObject, string propertyName, bool padding, params GUILayoutOption[] options)
    {
        SerializedProperty sp = serializedObject.FindProperty(propertyName);

        if (sp != null)
        {
            if (padding) EditorGUILayout.BeginHorizontal();

            if (sp.isArray && sp.type != "string")
            {
                DrawArray(serializedObject, propertyName, editorName ?? propertyName);
            }
            else if (editorName != null)
            {
                EditorGUILayout.PropertyField(sp, new GUIContent(editorName), options);
            }
            else
            {
                EditorGUILayout.PropertyField(sp, options);
            }

            if (padding)
            {
                EditorGUILayout.EndHorizontal();
            }
        }
        else
        {
            Debug.LogWarning("Unable to find property " + propertyName);
        }
        return sp;
    }
    public static void DrawArray(SerializedObject obj, string property, string title)
    {
        SerializedProperty sp = obj.FindProperty(property + ".Array.size");

        if (sp != null && DrawHeader(title))
        {
            BeginContents(false);
            int size = sp.intValue;
            int newSize = EditorGUILayout.IntField("Size", size);
            if (newSize != size) obj.FindProperty(property + ".Array.size").intValue = newSize;

            EditorGUI.indentLevel = 1;

            for (int i = 0; i < newSize; i++)
            {
                SerializedProperty p = obj.FindProperty(string.Format("{0}.Array.data[{1}]", property, i));
                if (p != null) EditorGUILayout.PropertyField(p);
            }
            EditorGUI.indentLevel = 0;
            EndContents();
        }
    }
    public static bool DrawHeader(string text)
    {
        return DrawHeader(text, text, false, false);
    }
    public static bool DrawHeader(string text, string key, bool forceOn, bool minimalistic)
    {
        bool state = EditorPrefs.GetBool(key, true);

        if (!minimalistic) GUILayout.Space(3f);
        if (!forceOn && !state) GUI.backgroundColor = new Color(0.8f, 0.8f, 0.8f);
        GUILayout.BeginHorizontal();
        GUI.changed = false;

        if (minimalistic)
        {
            if (state) text = "\u25BC" + (char)0x200a + text;
            else text = "\u25BA" + (char)0x200a + text;

            GUILayout.BeginHorizontal();
            GUI.contentColor = EditorGUIUtility.isProSkin ? new Color(1f, 1f, 1f, 0.7f) : new Color(0f, 0f, 0f, 0.7f);
            if (!GUILayout.Toggle(true, text, "PreToolbar2", GUILayout.MinWidth(20f))) state = !state;
            GUI.contentColor = Color.white;
            GUILayout.EndHorizontal();
        }
        else
        {
            text = "<b><size=11>" + text + "</size></b>";
            if (state) text = "\u25BC " + text;
            else text = "\u25BA " + text;
            if (!GUILayout.Toggle(true, text, "dragtab", GUILayout.MinWidth(20f))) state = !state;
        }

        if (GUI.changed)
        {
            EditorPrefs.SetBool(key, state);
        }

        if (!minimalistic) GUILayout.Space(2f);
        GUILayout.EndHorizontal();
        GUI.backgroundColor = Color.white;
        if (!forceOn && !state) GUILayout.Space(3f);
        return state;
    }


    public static void BeginContents() { BeginContents(GUISettingsBK.minimalisticLook); }
    public static void BeginContents(bool minimalistic)
    {
        if (!minimalistic)
        {
            mEndHorizontal = true;
            GUILayout.BeginHorizontal();
            EditorGUILayout.BeginHorizontal("Box", GUILayout.MinHeight(10f));
        }
        else
        {
            mEndHorizontal = false;
            EditorGUILayout.BeginHorizontal(GUILayout.MinHeight(10f));
            GUILayout.Space(10f);
        }
        GUILayout.BeginVertical();
        GUILayout.Space(2f);
    }
    static bool mEndHorizontal = false;
    static public void EndContents()
    {
        GUILayout.Space(3f);
        GUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();

        if (mEndHorizontal)
        {
            GUILayout.Space(3f);
            GUILayout.EndHorizontal();
        }

        GUILayout.Space(3f);
    }



    /// <summary>
    /// Load the asset at the specified path.
    /// </summary>
    static public Object LoadAsset(string path)
    {
        if (string.IsNullOrEmpty(path)) return null;
        return AssetDatabase.LoadMainAssetAtPath(path);
    }

    /// <summary>
    /// Convenience function to load an asset of specified type, given the full path to it.
    /// </summary>
    static public T LoadAsset<T>(string path) where T : Object
    {
        Object obj = LoadAsset(path);
        if (obj == null) return null;

        T val = obj as T;
        if (val != null) return val;

        if (typeof(T).IsSubclassOf(typeof(Component)))
        {
            if (obj.GetType() == typeof(GameObject))
            {
                GameObject go = obj as GameObject;
                return go.GetComponent(typeof(T)) as T;
            }
        }
        return null;
    }






}
