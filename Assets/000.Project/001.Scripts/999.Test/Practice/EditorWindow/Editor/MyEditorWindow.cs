using UnityEngine;
using UnityEditor;
using UnityEditor.Presets;

public class MyEditorWindow : EditorWindow
{
    private static class Styles
    {
        public static GUIContent presetIcon = EditorGUIUtility.IconContent("Preset.Context");
        public static GUIStyle iconButton = new GUIStyle("IconButton");
    }

    Editor _settingsEditor;
    MyWindowSettings _serializedSettings;

    public string someSettings
    {
        get { return EditorPrefs.GetString("MyEditorWindow_SomeSettings"); }
        set { EditorPrefs.SetString("MyEditorWindow_SomeSettings", value); }
    }

    [MenuItem("Window/MyEditorWindow")]
    static void OpenWindow()
    {
        GetWindow<MyEditorWindow>();
    }

    private void OnEnable()
    {
        _serializedSettings = ScriptableObject.CreateInstance<MyWindowSettings>();
        _serializedSettings.Init(this);
        _settingsEditor = Editor.CreateEditor(_serializedSettings);
    }

    private void OnDisable()
    {
        Object.DestroyImmediate(_serializedSettings);
        Object.DestroyImmediate(_settingsEditor);
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("My custom settings", EditorStyles.boldLabel);
        GUILayout.FlexibleSpace();
        Rect buttonPosition = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight, Styles.iconButton);

        if (EditorGUI.DropdownButton(buttonPosition, Styles.presetIcon, FocusType.Passive, Styles.iconButton))
        {
            MySettingsReceiver presetReceiver = ScriptableObject.CreateInstance<MySettingsReceiver>();
            presetReceiver.Init(_serializedSettings, this);
            PresetSelector.ShowSelector(_serializedSettings, null, true, presetReceiver);

        }
        EditorGUILayout.EndHorizontal();

        EditorGUI.BeginChangeCheck();
        _settingsEditor.OnInspectorGUI();
        if (EditorGUI.EndChangeCheck())
        {
            _serializedSettings.ApplySettings(this);
        }

    }





}
