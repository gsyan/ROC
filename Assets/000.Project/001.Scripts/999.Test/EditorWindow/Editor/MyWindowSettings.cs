using UnityEngine;

public class MyWindowSettings : ScriptableObject
{
    [SerializeField] string _someSettings;

    public void Init(MyEditorWindow window)
    {
        _someSettings = window.someSettings;
    }

    public void ApplySettings(MyEditorWindow window)
    {
        window.someSettings = _someSettings;
        window.Repaint();
    }
    


}
