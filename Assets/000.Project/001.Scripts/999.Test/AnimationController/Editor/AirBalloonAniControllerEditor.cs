using UnityEngine;
using UnityEditor;
using System;


namespace com.sbp.ai
{
    [CustomEditor(typeof(AirBalloonAniController))]
    [CanEditMultipleObjects]
    public class AirBalloonAniControllerEditor : Editor
    {
        AirBalloonAniController _script;

        SerializedProperty bAutoStart;
        SerializedProperty aniSpeed;
        SerializedProperty startFrame;
        SerializedProperty currentFrame;
        SerializedProperty currentClip;
        SerializedProperty currentClipTime;
        SerializedProperty currentClipTimeTotal;

        SerializedProperty aniClips;

        SerializedProperty steps;


        private bool bJumpTime = false;


        private void OnEnable()
        {
            _script = (AirBalloonAniController)target;

            bAutoStart = serializedObject.FindProperty("bAutoStart");
            aniSpeed = serializedObject.FindProperty("aniSpeed");
            startFrame = serializedObject.FindProperty("startFrame");
            currentFrame = serializedObject.FindProperty("currentFrame");
            currentClip = serializedObject.FindProperty("currentClip");
            currentClipTime = serializedObject.FindProperty("currentClipTime");
            currentClipTimeTotal = serializedObject.FindProperty("currentClipTimeTotal");
            

            aniClips = serializedObject.FindProperty("aniClips");
            steps = serializedObject.FindProperty("steps");
        }


        static bool showClips = false;
        static bool showSteps = true;
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(bAutoStart);
            EditorGUILayout.PropertyField(aniSpeed);
            EditorGUILayout.PropertyField(startFrame);
            EditorGUILayout.LabelField("currentFrame", _script.currentFrame.ToString());
            EditorGUILayout.LabelField("currentClip", _script.currentClip);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("currentClipTime", _script.currentClipTime.ToString());
            EditorGUILayout.LabelField("", "/" + _script.currentClipTimeTotal.ToString());
            EditorGUILayout.EndHorizontal();

            ListIteratorClip("aniClips", ref showClips);
            ListIteratorStep("steps", ref showSteps);

            serializedObject.ApplyModifiedProperties();


        }
        void ListIteratorClip(string propertyName, ref bool visible)
        {
            SerializedProperty listProperty = serializedObject.FindProperty(propertyName);
            visible = EditorGUILayout.Foldout(visible, listProperty.name);
            if (visible)
            {
                EditorGUI.indentLevel++;

                for (int i = 0; i < listProperty.arraySize; i++)
                {
                    SerializedProperty elementProperty = listProperty.GetArrayElementAtIndex(i);

                    EditorGUILayout.BeginHorizontal();

                    Rect drawZone1 = GUILayoutUtility.GetRect(0f, 16f);
                    EditorGUI.PropertyField(drawZone1, elementProperty, new GUIContent(""));
                    //bool showChildren = EditorGUI.PropertyField(drawZone1, elementProperty, new GUIContent(""));

                    //Rect drawZone2 = GUILayoutUtility.GetRect(0, 16f);
                    //EditorGUI.LabelField(drawZone2, "");

                    Rect drawZone3 = GUILayoutUtility.GetRect(0, 16f);
                    if (GUI.Button(drawZone3, "Jump"))
                    {
                        _script.JumpToClip(i);
                    }

                    EditorGUILayout.EndHorizontal();
                }

                ClipLengthAddSubtractButton();

                EditorGUI.indentLevel--;
            }
        }
        void ClipLengthAddSubtractButton()
        {
            EditorGUILayout.BeginHorizontal();


            if (_script.aniClips != null)
            {
                Rect drawZoneLabel = GUILayoutUtility.GetRect(80f, 16f);
                EditorGUI.LabelField(drawZoneLabel, "Steps Length: " + _script.aniClips.Length.ToString());
            }


            Rect drawZone1 = GUILayoutUtility.GetRect(3f, 16f);
            if (GUI.Button(drawZone1, "+"))
            {
                if (_script.aniClips != null)
                {
                    int count = _script.aniClips.Length;
                    Array.Resize<AnimationClip>(ref _script.aniClips, count + 1);
                    _script.aniClips[count] = null;
                }
                else
                {
                    _script.aniClips = new AnimationClip[1];
                }

            }

            Rect drawZone2 = GUILayoutUtility.GetRect(3f, 16f);
            if (GUI.Button(drawZone2, "-"))
            {
                if (_script.aniClips != null)
                {
                    int count = _script.aniClips.Length;
                    if (count > 0)
                    {
                        Array.Resize<AnimationClip>(ref _script.aniClips, count - 1);
                    }
                }
            }

            EditorGUILayout.EndHorizontal();
        }
        void ListIteratorStep(string propertyName, ref bool visible)
        {
            SerializedProperty listProperty = serializedObject.FindProperty(propertyName);
            visible = EditorGUILayout.Foldout(visible, listProperty.name);
            if (visible)
            {
                EditorGUI.indentLevel++;

                EditorGUILayout.BeginHorizontal();
                Rect drawZone4 = GUILayoutUtility.GetRect(0, 16f);
                EditorGUI.LabelField(drawZone4, "step");
                Rect drawZone5 = GUILayoutUtility.GetRect(0, 16f);
                EditorGUI.LabelField(drawZone5, "frame(0~1)");
                Rect drawZone6 = GUILayoutUtility.GetRect(0, 16f);
                EditorGUI.LabelField(drawZone6, "");
                EditorGUILayout.EndHorizontal();

                for (int i = 0; i < listProperty.arraySize; i++)
                {
                    SerializedProperty elementProperty = listProperty.GetArrayElementAtIndex(i);

                    EditorGUILayout.BeginHorizontal();

                    Rect drawZone1 = GUILayoutUtility.GetRect(0f, 16f);
                    bool showChildren = EditorGUI.PropertyField(drawZone1, elementProperty, new GUIContent(""));

                    Rect drawZone2 = GUILayoutUtility.GetRect(0, 16f);
                    EditorGUI.LabelField(drawZone2, "");

                    Rect drawZone3 = GUILayoutUtility.GetRect(0, 16f);
                    if (GUI.Button(drawZone3, "Jump"))
                    {
                        _script.ChangeJumpToFrame(_script.steps[i].stepValue);
                    }

                    EditorGUILayout.EndHorizontal();
                }

                StepLengthAddSubtractButton();

                EditorGUI.indentLevel--;
            }
        }
        void StepLengthAddSubtractButton()
        {
            EditorGUILayout.BeginHorizontal();

            
            if(_script.steps != null)
            {
                Rect drawZoneLabel = GUILayoutUtility.GetRect(80f, 16f);
                EditorGUI.LabelField(drawZoneLabel, "Steps Length: " + _script.steps.Length.ToString());
            }


            Rect drawZone1 = GUILayoutUtility.GetRect(3f, 16f);
            if (GUI.Button(drawZone1, "+"))
            {
                if (_script.steps != null)
                {
                    int count = _script.steps.Length;
                    Array.Resize<AirBalloonStep>(ref _script.steps, count + 1);
                    _script.steps[count] = new AirBalloonStep();
                    _script.steps[count].stepName = "step" + count.ToString();
                    _script.steps[count].stepValue = 0.0f;
                }
                else
                {
                    _script.steps = new AirBalloonStep[1];
                    _script.steps[0] = new AirBalloonStep();
                    _script.steps[0].stepName = "step0";
                    _script.steps[0].stepValue = 0.0f;
                }

            }

            Rect drawZone2 = GUILayoutUtility.GetRect(3f, 16f);
            if (GUI.Button(drawZone2, "-"))
            {
                if (_script.steps != null)
                {
                    int count = _script.steps.Length;
                    if (count > 0)
                    {
                        Array.Resize<AirBalloonStep>(ref _script.steps, count - 1);
                    }
                }
            }

            EditorGUILayout.EndHorizontal();
        }


    }
}

