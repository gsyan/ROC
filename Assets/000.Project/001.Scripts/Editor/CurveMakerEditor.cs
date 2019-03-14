using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(CurveMaker))]
public class CurveMakerEditor : Editor
{
    private CurveMaker script;

    SerializedProperty moveObject;
    SerializedProperty moveSpeed;

    SerializedProperty wayPointStart;
    SerializedProperty wayPointEnd;
    SerializedProperty wayPointCount;
    SerializedProperty wayPointDistance;

    SerializedProperty curvedPointCount;

    private void OnEnable()
    {
        script = target as CurveMaker;

        moveObject = serializedObject.FindProperty("moveObject");
        moveSpeed = serializedObject.FindProperty("moveSpeed");

        wayPointStart = serializedObject.FindProperty("wayPointStart");
        wayPointEnd = serializedObject.FindProperty("wayPointEnd");
        wayPointCount = serializedObject.FindProperty("wayPointCount");
        wayPointDistance = serializedObject.FindProperty("wayPointDistance");

        curvedPointCount = serializedObject.FindProperty("curvedPointCount");
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();//이걸 하면 모든 퍼블릭 프로퍼티 다 노출 됨

        if (script == null) { return; }

        serializedObject.Update();
        EditorGUILayout.PropertyField(moveObject);
        EditorGUILayout.PropertyField(moveSpeed);
        EditorGUILayout.PropertyField(wayPointStart);
        EditorGUILayout.PropertyField(wayPointEnd);
        EditorGUILayout.PropertyField(wayPointCount);
        EditorGUILayout.PropertyField(wayPointDistance);
        EditorGUILayout.PropertyField(curvedPointCount);
        
        if (GUILayout.Button("MakeWayPoints"))
        {
            ClearWayPointList();
            MakeWayPoints();
        }

        if (GUILayout.Button("ClearCurvedPoints"))
        {
            script.curvedPointList.Clear();
        }


        serializedObject.ApplyModifiedProperties();
    }
    private void ClearWayPointList()
    {
        for (int i = 0; i < script.wayPointList.Count; ++i)
        {
            if (i == 0 || i == script.wayPointList.Count - 1) { continue; }
            DestroyImmediate(script.wayPointList[i].gameObject);
        }
        script.wayPointList.Clear();
    }
    private void MakeWayPoints()
    {
        if (script.wayPointStart == null)
        {
            script.wayPointStart = GameObject.CreatePrimitive(PrimitiveType.Sphere).transform;
            script.wayPointStart.localScale = Vector3.one;
            script.wayPointStart.position = Vector3.zero;
        }
        if (script.wayPointEnd == null)
        {
            script.wayPointEnd = GameObject.CreatePrimitive(PrimitiveType.Sphere).transform;
            script.wayPointEnd.localScale = Vector3.one;
            script.wayPointEnd.position = Vector3.one * 10;
        }


        Vector3 direction = (script.wayPointEnd.position - script.wayPointStart.position).normalized;

        script.wayPointList.Add(script.wayPointStart);
        script.wayPointStart.SetParent(script.transform);
        GameObject obj;
        for (int i = 1; i < script.wayPointCount - 1; ++i)
        {
            obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            obj.transform.localScale = script.wayPointStart.localScale;
            obj.transform.position = script.wayPointList[i - 1].position + direction * script.wayPointDistance;
            obj.name = i.ToString();
            script.wayPointList.Add(obj.transform);
            obj.transform.SetParent(script.transform);
        }

        script.wayPointEnd.position = script.wayPointList[script.wayPointCount - 2].position + direction * script.wayPointDistance;
        script.wayPointEnd.localScale = script.wayPointStart.localScale;
        script.wayPointList.Add(script.wayPointEnd);
        script.wayPointEnd.SetParent(null);
        script.wayPointEnd.SetParent(script.transform);
    }





    //배열 항목을 Inspector 에 표시되도록 해주는 부분
    private bool bLlistVisibility01 = true;
    private bool bLlistVisibility02 = true;
    public void ListIterator(string propertyPath, ref bool visible)
    {
        SerializedProperty listProperty = serializedObject.FindProperty(propertyPath);
        visible = EditorGUILayout.Foldout(visible, listProperty.name);
        if (visible)
        {
            EditorGUI.indentLevel++;
            for (int i = 0; i < listProperty.arraySize; i++)
            {
                SerializedProperty elementProperty = listProperty.GetArrayElementAtIndex(i);
                Rect drawZone = GUILayoutUtility.GetRect(0f, 16f);
                bool showChildren = EditorGUI.PropertyField(drawZone, elementProperty);
            }
            EditorGUI.indentLevel--;
        }
    }



}
