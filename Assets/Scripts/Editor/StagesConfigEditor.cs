using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

// ��������� ����� ��������� ��� ������� ���� StagesConfig
[CustomEditor(typeof(StagesConfig))]
public class StagesConfigEditor : Editor
{
    // ��������������� �������� ��� ������ allStages
    SerializedProperty allStagesProperty;

    // �����, ������� ���������� ��� ��������� ��������� (��������, ��� ��������� ������� � ����������)
    private void OnEnable()
    {
        // ������� � ��������� ������ �� �������� "allStages"
        allStagesProperty = serializedObject.FindProperty("allStages");
    }

    // �������� �����, ������� ����������, ��� ����� ������������ GUI � ����������
    public override void OnInspectorGUI()
    {
        // ��������� ��������������� ������ ����� ������� ��������������
        serializedObject.Update();

        // ���������� �� ������� �������� ������ allStages
        for (int i = 0; i < allStagesProperty.arraySize; i++)
        {
            // �������� ������� ������� ������ allStages
            SerializedProperty StageConfigProperty = allStagesProperty.GetArrayElementAtIndex(i);

            // �������� ������������ ������ ��� ����������� ������������� ��������� � �����
            EditorGUILayout.BeginVertical("box");

            // ���������� ���� ��� �������� "main" (�������� �����������)
            EditorGUILayout.PropertyField(StageConfigProperty.FindPropertyRelative("main"));
            EditorGUILayout.PropertyField(StageConfigProperty.FindPropertyRelative("stageSound"));

            // �������� ����� ��� ����������� ������ ButtonData
            DrawButtonDataList(StageConfigProperty.FindPropertyRelative("buttons"));

            // ��������� ������ ��� �������� �������� StageConfig �� ������
            if (GUILayout.Button("Remove this StageConfig"))
            {
                // ������� ������� ������ �� �������
                allStagesProperty.DeleteArrayElementAtIndex(i);
            }

            // ��������� ������������ ������
            EditorGUILayout.EndVertical();

            // ��������� ������� ������������ ����� ��������
            EditorGUILayout.Space();
        }

        // ��������� ������ ��� �������� ������ StageConfig
        if (GUILayout.Button("Add new StageConfig"))
        {
            // �������� ����� ��� ���������� ������ �������� � ������ allStages
            AddNewStageConfig();
        }

        // ��������� ��������� � ��������������� �������
        serializedObject.ApplyModifiedProperties();
    }

    // ����� ��� ����������� ������ ButtonData (��������, scalable ��� unscalable)
    private void DrawButtonDataList(SerializedProperty listProperty)
    {
        // ���������� ��������� ������ � ������ �������
        EditorGUILayout.LabelField("Buttons", EditorStyles.boldLabel);

        // ���������� �� ������� �������� ������ ButtonData
        for (int i = 0; i < listProperty.arraySize; i++)
        {
            // �������� ������� ������� ������ ButtonData
            SerializedProperty buttonDataProperty = listProperty.GetArrayElementAtIndex(i);

            // �������� ��������� �������� ��� ����������� ������ ���������
            EditorGUI.BeginProperty(EditorGUILayout.GetControlRect(true), GUIContent.none, buttonDataProperty);

            // ���������� ���� ��� ������� ButtonData
            EditorGUILayout.PropertyField(buttonDataProperty.FindPropertyRelative("image"));
            EditorGUILayout.PropertyField(buttonDataProperty.FindPropertyRelative("position"));
            EditorGUILayout.PropertyField(buttonDataProperty.FindPropertyRelative("size"));
            EditorGUILayout.PropertyField(buttonDataProperty.FindPropertyRelative("isScalable"));
            EditorGUILayout.PropertyField(buttonDataProperty.FindPropertyRelative("title"));
            EditorGUILayout.PropertyField(buttonDataProperty.FindPropertyRelative("effects"));
            EditorGUILayout.PropertyField(buttonDataProperty.FindPropertyRelative("isDragable"));
            EditorGUILayout.PropertyField(buttonDataProperty.FindPropertyRelative("clickSound"));
            EditorGUILayout.PropertyField(buttonDataProperty.FindPropertyRelative("longPressSound"));

            // �������� �������������� ������ ��� ���������� ������ �� ����� �����
            EditorGUILayout.BeginHorizontal();

            // ������ ��� ����������� ������� � ������� �� ����������� �������
            if (GUILayout.Button("Copy from selected Stage"))
            {
                // �������� ����� ��� ����������� ������ �� ����������� �������
                CopyFromSelectedStage(buttonDataProperty);
            }

            // ������ ��� �������� �������� ButtonData �� ������
            if (GUILayout.Button("Remove"))
            {
                // ������� ������� ������ �� �������
                listProperty.DeleteArrayElementAtIndex(i);
            }

            // ��������� �������������� ������
            EditorGUILayout.EndHorizontal();

            // ��������� ������� ������������ ����� ����������
            EditorGUILayout.Space();

            // ��������� ��������� ��������
            EditorGUI.EndProperty();
        }

        // ��������� ������ ��� ���������� ������ �������� ButtonData � ������
        if (GUILayout.Button("Add new ButtonData"))
        {
            // ����������� ������ �������, �������� ����� �������
            listProperty.arraySize++;
        }
    }

    // ����� ��� ���������� ������ StageConfig � ������ allStages
    private void AddNewStageConfig()
    {
        // ����������� ������ ������� allStages
        allStagesProperty.arraySize++;
        // �������� ������ �� ����� ������� �������
        SerializedProperty newStageConfig = allStagesProperty.GetArrayElementAtIndex(allStagesProperty.arraySize - 1);

        // ������� ���� ������ StageConfig
        newStageConfig.FindPropertyRelative("main").objectReferenceValue = null;
        newStageConfig.FindPropertyRelative("stageSound").objectReferenceValue = null;
        newStageConfig.FindPropertyRelative("buttons").ClearArray();
    }

    // ����� ��� ����������� ������� � ������� �� ����������� Stage
    private void CopyFromSelectedStage(SerializedProperty buttonDataProperty)
    {
        // �������� ���������� ������
        GameObject selectedObject = Selection.activeGameObject;

        // ���������, ���� ������ �������
        if (selectedObject != null)
        {
            // �������� ��������� RectTransform � ����������� �������
            RectTransform rectTransform = selectedObject.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                // �������� ������� � ������ � ��������������� �������� ButtonData
                buttonDataProperty.FindPropertyRelative("position").vector2Value = rectTransform.anchoredPosition;
                buttonDataProperty.FindPropertyRelative("size").vector2Value = rectTransform.sizeDelta;

                // ��������� ���������
                serializedObject.ApplyModifiedProperties();
                Debug.Log("Position and size copied from selected Stage.");
            }
            else
            {
                Debug.LogWarning("Selected object does not have a RectTransform component.");
            }
        }
        else
        {
            Debug.LogWarning("No GameObject selected.");
        }
    }
}
