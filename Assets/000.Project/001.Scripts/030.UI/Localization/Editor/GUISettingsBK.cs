﻿
using UnityEngine;
using UnityEditor;

public class GUISettingsBK
{
    public enum ColorMode
    {
        Orange,
        Green,
        Blue,
    }

    #region Generic Get and Set methods
    /// <summary>
    /// Save the specified boolean value in settings.
    /// </summary>

    static public void SetBool(string name, bool val) { EditorPrefs.SetBool(name, val); }

    /// <summary>
    /// Save the specified integer value in settings.
    /// </summary>

    static public void SetInt(string name, int val) { EditorPrefs.SetInt(name, val); }

    /// <summary>
    /// Save the specified float value in settings.
    /// </summary>

    static public void SetFloat(string name, float val) { EditorPrefs.SetFloat(name, val); }

    /// <summary>
    /// Save the specified string value in settings.
    /// </summary>

    static public void SetString(string name, string val) { EditorPrefs.SetString(name, val); }

    /// <summary>
    /// Save the specified color value in settings.
    /// </summary>

    static public void SetColor(string name, Color c) { SetString(name, c.r + " " + c.g + " " + c.b + " " + c.a); }

    /// <summary>
    /// Save the specified enum value to settings.
    /// </summary>

    static public void SetEnum(string name, System.Enum val) { SetString(name, val.ToString()); }

    /// <summary>
    /// Save the specified object in settings.
    /// </summary>

    static public void Set(string name, Object obj)
    {
        if (obj == null)
        {
            EditorPrefs.DeleteKey(name);
        }
        else
        {
            if (obj != null)
            {
                string path = AssetDatabase.GetAssetPath(obj);

                if (!string.IsNullOrEmpty(path))
                {
                    EditorPrefs.SetString(name, path);
                }
                else
                {
                    EditorPrefs.SetString(name, obj.GetInstanceID().ToString());
                }
            }
            else EditorPrefs.DeleteKey(name);
        }
    }

    /// <summary>
    /// Get the previously saved boolean value.
    /// </summary>

    static public bool GetBool(string name, bool defaultValue) { return EditorPrefs.GetBool(name, defaultValue); }

    /// <summary>
    /// Get the previously saved integer value.
    /// </summary>

    static public int GetInt(string name, int defaultValue) { return EditorPrefs.GetInt(name, defaultValue); }

    /// <summary>
    /// Get the previously saved float value.
    /// </summary>

    static public float GetFloat(string name, float defaultValue) { return EditorPrefs.GetFloat(name, defaultValue); }

    /// <summary>
    /// Get the previously saved string value.
    /// </summary>

    static public string GetString(string name, string defaultValue) { return EditorPrefs.GetString(name, defaultValue); }

    /// <summary>
    /// Get a previously saved color value.
    /// </summary>

    static public Color GetColor(string name, Color c)
    {
        string strVal = GetString(name, c.r + " " + c.g + " " + c.b + " " + c.a);
        string[] parts = strVal.Split(' ');

        if (parts.Length == 4)
        {
            float.TryParse(parts[0], out c.r);
            float.TryParse(parts[1], out c.g);
            float.TryParse(parts[2], out c.b);
            float.TryParse(parts[3], out c.a);
        }
        return c;
    }

    /// <summary>
    /// Get a previously saved enum from settings.
    /// </summary>

    static public T GetEnum<T>(string name, T defaultValue)
    {
        string val = GetString(name, defaultValue.ToString());
        string[] names = System.Enum.GetNames(typeof(T));
        System.Array values = System.Enum.GetValues(typeof(T));

        for (int i = 0; i < names.Length; ++i)
        {
            if (names[i] == val)
                return (T)values.GetValue(i);
        }
        return defaultValue;
    }

    /// <summary>
    /// Get a previously saved object from settings.
    /// </summary>

    static public T Get<T>(string name, T defaultValue) where T : Object
    {
        string path = EditorPrefs.GetString(name);
        if (string.IsNullOrEmpty(path)) return null;

        T retVal = GUIEditorToolsBK.LoadAsset<T>(path);

        if (retVal == null)
        {
            int id;
            if (int.TryParse(path, out id))
                return EditorUtility.InstanceIDToObject(id) as T;
        }
        return retVal;
    }
    #endregion



    #region Convenience accessor properties

    static public bool showTransformHandles
    {
        get { return GetBool("NGUI Transform Handles", false); }
        set { SetBool("NGUI Transform Handles", value); }
    }

    static public bool minimalisticLook
    {
        get { return GetBool("NGUI Minimalistic", false); }
        set { SetBool("NGUI Minimalistic", value); }
    }

    static public bool unifiedTransform
    {
        get { return GetBool("NGUI Unified", false); }
        set { SetBool("NGUI Unified", value); }
    }

    static public Color color
    {
        get { return GetColor("NGUI Color", Color.white); }
        set { SetColor("NGUI Color", value); }
    }

    static public Color foregroundColor
    {
        get { return GetColor("NGUI FG Color", Color.white); }
        set { SetColor("NGUI FG Color", value); }
    }

    static public Color backgroundColor
    {
        get { return GetColor("NGUI BG Color", Color.black); }
        set { SetColor("NGUI BG Color", value); }
    }

    static public ColorMode colorMode
    {
        get { return GetEnum("NGUI Color Mode", ColorMode.Blue); }
        set { SetEnum("NGUI Color Mode", value); }
    }



    #endregion





}
