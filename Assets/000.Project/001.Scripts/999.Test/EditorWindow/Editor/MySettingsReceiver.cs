using UnityEditor.Presets;

public class MySettingsReceiver : PresetSelectorReceiver
{
    Preset initialValues;
    MyWindowSettings currentSettings;
    MyEditorWindow currentWindow;

    public void Init(MyWindowSettings settings, MyEditorWindow window)
    {
        currentWindow = window;
        currentSettings = settings;
        initialValues = new Preset(currentSettings);
    }

    public override void OnSelectionChanged(Preset selection)
    {
        //base.OnSelectionChanged(selection);

        if( selection != null)
        {
            selection.ApplyTo(currentSettings);
        }
        else
        {
            initialValues.ApplyTo(currentSettings);
        }

        currentSettings.ApplySettings(currentWindow);
    }

    public override void OnSelectionClosed(Preset selection)
    {
        //base.OnSelectionClosed(selection);

        OnSelectionChanged(selection);
        DestroyImmediate(this);

    }









}
