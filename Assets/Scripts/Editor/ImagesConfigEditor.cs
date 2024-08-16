using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

// ��������� ����� ��������� ��� ������� ���� ImagesConfig
[CustomEditor(typeof(ImagesConfig))]
public class ImagesConfigEditor : Editor
{
    // ��������������� �������� ��� ������ allImages
    SerializedProperty allImagesProperty;

    // �����, ������� ���������� ��� ��������� ��������� (��������, ��� ��������� ������� � ����������)
    private void OnEnable()
    {
        // ������� � ��������� ������ �� �������� "allImages"
        allImagesProperty = serializedObject.FindProperty("allImages");
    }

    // �������� �����, ������� ����������, ��� ����� ������������ GUI � ����������
    public override void OnInspectorGUI()
    {
        // ��������� ��������������� ������ ����� ������� ��������������
        serializedObject.Update();

        // ���������� �� ������� �������� ������ allImages
        for (int i = 0; i < allImagesProperty.arraySize; i++)
        {
            // �������� ������� ������� ������ allImages
            SerializedProperty imageConfigProperty = allImagesProperty.GetArrayElementAtIndex(i);

            // �������� ������������ ������ ��� ����������� ������������� ��������� � �����
            EditorGUILayout.BeginVertical("box");

            // ���������� ���� ��� �������� "main" (�������� �����������)
            EditorGUILayout.PropertyField(imageConfigProperty.FindPropertyRelative("main"));

            // �������� ����� ��� ����������� ������ ButtonData
            DrawButtonDataList(imageConfigProperty.FindPropertyRelative("buttons"));

            // ��������� ������ ��� �������� �������� ImageConfig �� ������
            if (GUILayout.Button("Remove this ImageConfig"))
            {
                // ������� ������� ������ �� �������
                allImagesProperty.DeleteArrayElementAtIndex(i);
            }

            // ��������� ������������ ������
            EditorGUILayout.EndVertical();

            // ��������� ������� ������������ ����� ��������
            EditorGUILayout.Space();
        }

        // ��������� ������ ��� �������� ������ ImageConfig
        if (GUILayout.Button("Add new ImageConfig"))
        {
            // �������� ����� ��� ���������� ������ �������� � ������ allImages
            AddNewImageConfig();
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
            EditorGUILayout.PropertyField(buttonDataProperty.FindPropertyRelative("SomeUsefulData"));

            // �������� �������������� ������ ��� ���������� ������ �� ����� �����
            EditorGUILayout.BeginHorizontal();

            // ������ ��� ����������� ������� � ������� �� ����������� �������
            if (GUILayout.Button("Copy from selected Image"))
            {
                // �������� ����� ��� ����������� ������ �� ����������� �������
                CopyFromSelectedImage(buttonDataProperty);
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

    // ����� ��� ���������� ������ ImageConfig � ������ allImages
    private void AddNewImageConfig()
    {
        // ����������� ������ ������� allImages
        allImagesProperty.arraySize++;
        // �������� ������ �� ����� ������� �������
        SerializedProperty newImageConfig = allImagesProperty.GetArrayElementAtIndex(allImagesProperty.arraySize - 1);

        // ������� ���� ������ ImageConfig
        newImageConfig.FindPropertyRelative("main").objectReferenceValue = null;
        newImageConfig.FindPropertyRelative("buttons").ClearArray();
    }

    // ����� ��� ����������� ������� � ������� �� ����������� Image
    private void CopyFromSelectedImage(SerializedProperty buttonDataProperty)
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
                Debug.Log("Position and size copied from selected Image.");
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
