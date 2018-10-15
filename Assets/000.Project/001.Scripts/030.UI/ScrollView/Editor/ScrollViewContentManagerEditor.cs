using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(ScrollViewContent))]
public class ScrollViewContentEditor : Editor
{
    private ScrollViewContent script;

    SerializedProperty bVirtical;
    SerializedProperty contentsWidth;
    SerializedProperty contentsHeight;
    SerializedProperty gapSize;


    private void OnEnable()
    {
        script = target as ScrollViewContent;

        bVirtical = serializedObject.FindProperty("bVirtical");
        contentsWidth = serializedObject.FindProperty("contentsWidth");
        contentsHeight = serializedObject.FindProperty("contentsHeight");
        gapSize = serializedObject.FindProperty("gapSize");

    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();//이걸 하면 모든 퍼블릭 프로퍼티 다 노출 됨

        if (script == null) { return; }

        serializedObject.Update();

        EditorGUILayout.PropertyField(bVirtical);
        EditorGUILayout.PropertyField(contentsWidth);
        EditorGUILayout.PropertyField(contentsHeight);
        EditorGUILayout.PropertyField(gapSize);

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("SaveSizeOfFirst"))
        {
            SaveSizeOfFirst();
        }
        if (GUILayout.Button("Realignment"))
        {
            Realignment();
        }
        GUILayout.EndHorizontal();




        serializedObject.ApplyModifiedProperties();
    }


    private void SaveSizeOfFirst()
    {
        if(script.transform.childCount > 0)
        {
            RectTransform rt = script.transform.GetChild(0).GetComponent<RectTransform>();
            script.contentsWidth = rt.sizeDelta.x;
            script.contentsHeight = rt.sizeDelta.y;
        }

    }
    private void Realignment()
    {
        RectTransform rt;
        Vector2 position = Vector2.zero;
        Vector2 positionAdd = new Vector2(0, -script.contentsHeight - script.gapSize);
        if(!script.bVirtical)
        {
            positionAdd = new Vector2(script.contentsWidth + script.gapSize, 0);
        }
        Vector2 size = new Vector2(script.contentsWidth, script.contentsHeight);

        for (int i=0; i < script.transform.childCount; ++i)
        {
            rt = script.transform.GetChild(i).GetComponent<RectTransform>();
            rt.sizeDelta = size;
            rt.anchoredPosition = position;
            position += positionAdd;
        }

        size = script.GetComponent<RectTransform>().sizeDelta;
        if (script.bVirtical)
        {
            position.x = 0;
            position.y = -position.y;
            size = position;
            if( size.y > script.transform.parent.parent.GetComponent<RectTransform>().sizeDelta.y )
            {
                script.GetComponent<RectTransform>().pivot = new Vector2(0, 0);
            }
            else
            {
                script.GetComponent<RectTransform>().pivot = new Vector2(0, 1);
            }
        }
        else
        {
            position.y = 0;
            size = position;
            if (size.x > script.transform.parent.parent.GetComponent<RectTransform>().sizeDelta.x)
            {
                script.GetComponent<RectTransform>().pivot = new Vector2(1, 1);
            }
            else
            {
                script.GetComponent<RectTransform>().pivot = new Vector2(0, 1);
            }
        }
        script.GetComponent<RectTransform>().sizeDelta = size;

        
    }



}
