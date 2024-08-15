using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(ImagesConfig))]
public class ImagesConfigEditor : Editor
{
    SerializedProperty allImagesProperty;

    private void OnEnable()
    {
        allImagesProperty = serializedObject.FindProperty("allImages");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        for (int i = 0; i < allImagesProperty.arraySize; i++)
        {
            SerializedProperty imageConfigProperty = allImagesProperty.GetArrayElementAtIndex(i);

            EditorGUILayout.BeginVertical("box"); 

            EditorGUILayout.PropertyField(imageConfigProperty.FindPropertyRelative("main"));

            DrawButtonDataList(imageConfigProperty.FindPropertyRelative("buttons"), "Buttons");

            if (GUILayout.Button("Remove this ImageConfig"))
            {
                allImagesProperty.DeleteArrayElementAtIndex(i);
            }

            EditorGUILayout.EndVertical();

            EditorGUILayout.Space(); 
        }

        if (GUILayout.Button("Add new ImageConfig"))
        {
            AddNewImageConfig();
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawButtonDataList(SerializedProperty listProperty, string label)
    {
        EditorGUILayout.LabelField(label, EditorStyles.boldLabel);

        for (int i = 0; i < listProperty.arraySize; i++)
        {
            SerializedProperty buttonDataProperty = listProperty.GetArrayElementAtIndex(i);

            EditorGUI.BeginProperty(EditorGUILayout.GetControlRect(true), GUIContent.none, buttonDataProperty);

            EditorGUILayout.PropertyField(buttonDataProperty.FindPropertyRelative("image"));
            EditorGUILayout.PropertyField(buttonDataProperty.FindPropertyRelative("position"));
            EditorGUILayout.PropertyField(buttonDataProperty.FindPropertyRelative("size"));
            EditorGUILayout.PropertyField(buttonDataProperty.FindPropertyRelative("isScalable"));
            EditorGUILayout.PropertyField(buttonDataProperty.FindPropertyRelative("SomeUsefulData"));

            EditorGUILayout.BeginHorizontal(); 
            
            if (GUILayout.Button("Copy from selected Image"))
            {
                CopyFromSelectedImage(buttonDataProperty);
            }

            
            if (GUILayout.Button("Remove"))
            {
                listProperty.DeleteArrayElementAtIndex(i);
            }

            EditorGUILayout.EndHorizontal(); 

            EditorGUILayout.Space(); 

            EditorGUI.EndProperty();
        }

        if (GUILayout.Button($"Add new {label} ButtonData"))
        {
            listProperty.arraySize++;
        }
    }

    private void AddNewImageConfig()
    {
        allImagesProperty.arraySize++;
        SerializedProperty newImageConfig = allImagesProperty.GetArrayElementAtIndex(allImagesProperty.arraySize - 1);

        newImageConfig.FindPropertyRelative("main").objectReferenceValue = null;
        newImageConfig.FindPropertyRelative("buttons").ClearArray();
    }

    private void CopyFromSelectedImage(SerializedProperty buttonDataProperty)
    {
        GameObject selectedObject = Selection.activeGameObject;

        if (selectedObject != null)
        {
            RectTransform rectTransform = selectedObject.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                buttonDataProperty.FindPropertyRelative("position").vector2Value = rectTransform.anchoredPosition;
                buttonDataProperty.FindPropertyRelative("size").vector2Value = rectTransform.sizeDelta;

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
