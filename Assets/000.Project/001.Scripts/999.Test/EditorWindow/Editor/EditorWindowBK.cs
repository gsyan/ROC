using UnityEngine;
using UnityEditor;

/// <summary>
/// 참고: https://docs.unity3d.com/kr/current/ScriptReference/EditorWindow.html
/// </summary>

public class EditorWindowBK : EditorWindow
{
    
    bool groupEnabled;
    bool myBool = true;
    float myFloat = 1.23f;


    [MenuItem("Window/EditorWindowBK")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(EditorWindowBK));
    }

    /// Called as the new window is opened.
    private void Awake()
    {
        //Debug.Log("EditorWindowBK Awake()");
    }

    /// OnDestroy is called to close the EditorWindow window.
    private void OnDestroy()
    {
        
    }

    //Called when the window gets keyboard focus.
    private void OnFocus()
    {
        //Debug.Log("EditorWindowBK OnFocus()");
    }

    //Implement your own editor GUI here.
    private void OnGUI()
    {
        WriteLabel();
        ToggleGroup();
        MakeWindow();
        GUILayout.Space(110);
        Notification();

        // 변수
        // focusedWindow : The EditorWindow which currently has keyboard focus. (Read Only)
        // mouseOverWindow : The EditorWindow currently under the mouse cursor. (Read Only)
        // autoRepaintOnSceneChange = false 인데 true 로 바꿔서 써먹을 수 있음
        // maximized : Is this window maximized?
        // maxSize : The maximum size of this window.
        // minSize : The minimum size of this window.
        // position : The desired position of the window in screen space.
        // titleContent : The GUIContent used for drawing the title of EditorWindows.
        // wantsMouseEnterLeaveWindow : Checks whether MouseEnterWindow and MouseLeaveWindow events are received in the GUI in this Editor window.
        // wantsMouseMove : Checks whether MouseMove events are received in the GUI in this Editor window.




        //Close();//Close the editor window.
        //Focus();//FocusExample2.Instance.Focus(); 과 같이 사용, class FocusExample2 : EditorWindow




    }
    private void WriteLabel()
    {
        string myString = "Hello World";
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        myString = EditorGUILayout.TextField("Text Field", myString);
    }
    private void ToggleGroup()
    {
        groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
        myBool = EditorGUILayout.Toggle("Toggle", myBool);
        myFloat = EditorGUILayout.Slider("Slider", myFloat, -3, 3);
        EditorGUILayout.EndToggleGroup();
    }
    private void MakeWindow()
    {
        // All GUI.Window or GUILayout.Window must come inside a BeginWindows / EndWindows pair
        BeginWindows();
        Rect windowRect = new Rect(0, 100, 200, 100);
        windowRect = GUILayout.Window(1, windowRect, DoWindow, "Hi There");
        EndWindows();
    }
    void DoWindow(int unusedWindowID)
    {
        GUILayout.Button("Hi");
        GUI.DragWindow();
    }
    private void Notification()
    {
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Show Notification"))
        {
            ShowNotification(new GUIContent("This is a Notification"));
        }
        if (GUILayout.Button("Remove Notification"))
        {
            RemoveNotification();
        }
        EditorGUILayout.EndHorizontal();
    }
    





    //Handler for message that is sent when an object or group of objects in the hierarchy changes.
    private void OnHierarchyChange()
    {
        //Debug.Log("EditorWindowBK OnHierarchyChange()");
    }

    //OnInspectorUpdate is called at 10 frames per second to give the inspector a chance to update.
    private void OnInspectorUpdate()
    {
        
    }

    //Called when the window loses keyboard focus.
    private void OnLostFocus()
    {
        //Debug.Log("EditorWindowBK OnLostFocus()");
    }

    //Handler for message that is sent whenever the state of the project changes.
    private void OnProjectChange()
    {
        //Debug.Log("EditorWindowBK OnProjectChange()");
    }

    //Called whenever the selection has changed.( at hierarcy and project )
    private void OnSelectionChange()
    {
        //Debug.Log("EditorWindowBK OnSelectionChange()");
    }

    ////Called multiple times per second on all visible windows.( 필요 시 주석 풀기 )
    //private void Update()
    //{
    //    //Debug.Log("EditorWindowBK Update()");
    //}


}
