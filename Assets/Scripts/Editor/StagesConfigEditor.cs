using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

// ��������� ����� ��������� ��� ������� ���� StagesConfig
[CustomEditor(typeof(StagesConfig))]
public class StagesConfigEditor : Editor
{
    // ��������������� �������� ��� ������ allStages
    SerializedProperty allStagesProperty;
    private string datastring = "";

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
            EditorGUILayout.PropertyField(StageConfigProperty.FindPropertyRelative("music"));

            // �������� ����� ��� ����������� ������ ActorData
            DrawActorDataList(StageConfigProperty.FindPropertyRelative("actors"));
            EditorGUILayout.PropertyField(StageConfigProperty.FindPropertyRelative("main_url"));
            EditorGUILayout.PropertyField(StageConfigProperty.FindPropertyRelative("music_url"));

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
        if (GUILayout.Button("Add new Stage"))
        {
            // �������� ����� ��� ���������� ������ �������� � ������ allStages
            AddNewStageConfig();
        }

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        if (GUILayout.Button("Serialize"))
        {
            datastring = JsonUtility.ToJson(new StagesConfigSerializable((StagesConfig)target), true);
        }
        GUILayout.TextField(datastring);
        if (GUILayout.Button("Deserialize"))
        {
            JsonUtility.FromJson<StagesConfigSerializable>(datastring).FillStagesConfig((StagesConfig)target);
        }

        // ��������� ��������� � ��������������� �������
        serializedObject.ApplyModifiedProperties();
    }

    // ����� ��� ����������� ������ ActorData (��������, scalable ��� unscalable)
    private void DrawActorDataList(SerializedProperty listProperty)
    {
        // ���������� ��������� ������ � ������ �������
        EditorGUILayout.LabelField("Actors", EditorStyles.boldLabel);

        // ���������� �� ������� �������� ������ ActorData
        for (int i = 0; i < listProperty.arraySize; i++)
        {
            // �������� ������� ������� ������ ActorData
            SerializedProperty ActorDataProperty = listProperty.GetArrayElementAtIndex(i);

            // �������� ��������� �������� ��� ����������� ������ ���������
            EditorGUI.BeginProperty(EditorGUILayout.GetControlRect(true), GUIContent.none, ActorDataProperty);

            // ���������� ���� ��� ������� ActorData
            EditorGUILayout.PropertyField(ActorDataProperty.FindPropertyRelative("image"));
            EditorGUILayout.PropertyField(ActorDataProperty.FindPropertyRelative("position"));
            EditorGUILayout.PropertyField(ActorDataProperty.FindPropertyRelative("size"));
            EditorGUILayout.PropertyField(ActorDataProperty.FindPropertyRelative("isScalable"));
            EditorGUILayout.PropertyField(ActorDataProperty.FindPropertyRelative("title"));
            EditorGUILayout.PropertyField(ActorDataProperty.FindPropertyRelative("effects"));
            EditorGUILayout.PropertyField(ActorDataProperty.FindPropertyRelative("isDragable"));
            EditorGUILayout.PropertyField(ActorDataProperty.FindPropertyRelative("clickSound"));
            EditorGUILayout.PropertyField(ActorDataProperty.FindPropertyRelative("longPressSound"));
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(ActorDataProperty.FindPropertyRelative("image_url"));
            EditorGUILayout.PropertyField(ActorDataProperty.FindPropertyRelative("clickSound_url"));
            EditorGUILayout.PropertyField(ActorDataProperty.FindPropertyRelative("longPressSound_url"));


            // �������� �������������� ������ ��� ���������� ������ �� ����� �����
            EditorGUILayout.BeginHorizontal();

            // ������ ��� ����������� ������� � ������� �� ����������� �������
            if (GUILayout.Button("Copy from selected"))
            {
                // �������� ����� ��� ����������� ������ �� ����������� �������
                CopyFromSelectedStage(ActorDataProperty);
            }

            // ������ ��� �������� �������� ActorData �� ������
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

        // ��������� ������ ��� ���������� ������ �������� ActorData � ������
        if (GUILayout.Button("Add new Actor"))
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
        newStageConfig.FindPropertyRelative("music").objectReferenceValue = null;
        newStageConfig.FindPropertyRelative("actors").ClearArray();
    }

    // ����� ��� ����������� ������� � ������� �� ����������� Stage
    private void CopyFromSelectedStage(SerializedProperty ActorDataProperty)
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
                // �������� ������� � ������ � ��������������� �������� ActorData
                ActorDataProperty.FindPropertyRelative("position").vector2Value = rectTransform.anchoredPosition;
                ActorDataProperty.FindPropertyRelative("size").vector2Value = rectTransform.sizeDelta;

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
