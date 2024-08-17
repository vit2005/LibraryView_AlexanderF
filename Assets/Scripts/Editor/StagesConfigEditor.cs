using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

// Объявляем класс редактора для объекта типа StagesConfig
[CustomEditor(typeof(StagesConfig))]
public class StagesConfigEditor : Editor
{
    // Сериализованное свойство для списка allStages
    SerializedProperty allStagesProperty;

    // Метод, который вызывается при активации редактора (например, при выделении объекта в инспекторе)
    private void OnEnable()
    {
        // Находим и сохраняем ссылку на свойство "allStages"
        allStagesProperty = serializedObject.FindProperty("allStages");
    }

    // Основной метод, который определяет, как будет отображаться GUI в инспекторе
    public override void OnInspectorGUI()
    {
        // Обновляем сериализованный объект перед началом редактирования
        serializedObject.Update();

        // Проходимся по каждому элементу списка allStages
        for (int i = 0; i < allStagesProperty.arraySize; i++)
        {
            // Получаем текущий элемент списка allStages
            SerializedProperty StageConfigProperty = allStagesProperty.GetArrayElementAtIndex(i);

            // Начинаем вертикальную группу для визуального группирования элементов в рамку
            EditorGUILayout.BeginVertical("box");

            // Отображаем поле для свойства "main" (основное изображение)
            EditorGUILayout.PropertyField(StageConfigProperty.FindPropertyRelative("main"));
            EditorGUILayout.PropertyField(StageConfigProperty.FindPropertyRelative("stageSound"));

            // Вызываем метод для отображения списка ButtonData
            DrawButtonDataList(StageConfigProperty.FindPropertyRelative("buttons"));

            // Добавляем кнопку для удаления текущего StageConfig из списка
            if (GUILayout.Button("Remove this StageConfig"))
            {
                // Удаляем элемент списка по индексу
                allStagesProperty.DeleteArrayElementAtIndex(i);
            }

            // Завершаем вертикальную группу
            EditorGUILayout.EndVertical();

            // Добавляем немного пространства между группами
            EditorGUILayout.Space();
        }

        // Добавляем кнопку для создания нового StageConfig
        if (GUILayout.Button("Add new StageConfig"))
        {
            // Вызываем метод для добавления нового элемента в список allStages
            AddNewStageConfig();
        }

        // Применяем изменения в сериализованном объекте
        serializedObject.ApplyModifiedProperties();
    }

    // Метод для отображения списка ButtonData (например, scalable или unscalable)
    private void DrawButtonDataList(SerializedProperty listProperty)
    {
        // Отображаем заголовок списка с жирным шрифтом
        EditorGUILayout.LabelField("Buttons", EditorStyles.boldLabel);

        // Проходимся по каждому элементу списка ButtonData
        for (int i = 0; i < listProperty.arraySize; i++)
        {
            // Получаем текущий элемент списка ButtonData
            SerializedProperty buttonDataProperty = listProperty.GetArrayElementAtIndex(i);

            // Начинаем обработку свойства для возможности отката изменений
            EditorGUI.BeginProperty(EditorGUILayout.GetControlRect(true), GUIContent.none, buttonDataProperty);

            // Отображаем поля для свойств ButtonData
            EditorGUILayout.PropertyField(buttonDataProperty.FindPropertyRelative("image"));
            EditorGUILayout.PropertyField(buttonDataProperty.FindPropertyRelative("position"));
            EditorGUILayout.PropertyField(buttonDataProperty.FindPropertyRelative("size"));
            EditorGUILayout.PropertyField(buttonDataProperty.FindPropertyRelative("isScalable"));
            EditorGUILayout.PropertyField(buttonDataProperty.FindPropertyRelative("title"));
            EditorGUILayout.PropertyField(buttonDataProperty.FindPropertyRelative("effects"));
            EditorGUILayout.PropertyField(buttonDataProperty.FindPropertyRelative("isDragable"));
            EditorGUILayout.PropertyField(buttonDataProperty.FindPropertyRelative("clickSound"));
            EditorGUILayout.PropertyField(buttonDataProperty.FindPropertyRelative("longPressSound"));

            // Начинаем горизонтальную группу для размещения кнопок на одной линии
            EditorGUILayout.BeginHorizontal();

            // Кнопка для копирования позиции и размера из выделенного объекта
            if (GUILayout.Button("Copy from selected Stage"))
            {
                // Вызываем метод для копирования данных из выделенного объекта
                CopyFromSelectedStage(buttonDataProperty);
            }

            // Кнопка для удаления текущего ButtonData из списка
            if (GUILayout.Button("Remove"))
            {
                // Удаляем элемент списка по индексу
                listProperty.DeleteArrayElementAtIndex(i);
            }

            // Завершаем горизонтальную группу
            EditorGUILayout.EndHorizontal();

            // Добавляем немного пространства между элементами
            EditorGUILayout.Space();

            // Завершаем обработку свойства
            EditorGUI.EndProperty();
        }

        // Добавляем кнопку для добавления нового элемента ButtonData в список
        if (GUILayout.Button("Add new ButtonData"))
        {
            // Увеличиваем размер массива, добавляя новый элемент
            listProperty.arraySize++;
        }
    }

    // Метод для добавления нового StageConfig в список allStages
    private void AddNewStageConfig()
    {
        // Увеличиваем размер массива allStages
        allStagesProperty.arraySize++;
        // Получаем ссылку на новый элемент массива
        SerializedProperty newStageConfig = allStagesProperty.GetArrayElementAtIndex(allStagesProperty.arraySize - 1);

        // Очищаем поля нового StageConfig
        newStageConfig.FindPropertyRelative("main").objectReferenceValue = null;
        newStageConfig.FindPropertyRelative("stageSound").objectReferenceValue = null;
        newStageConfig.FindPropertyRelative("buttons").ClearArray();
    }

    // Метод для копирования позиции и размера из выделенного Stage
    private void CopyFromSelectedStage(SerializedProperty buttonDataProperty)
    {
        // Получаем выделенный объект
        GameObject selectedObject = Selection.activeGameObject;

        // Проверяем, если объект выделен
        if (selectedObject != null)
        {
            // Получаем компонент RectTransform у выделенного объекта
            RectTransform rectTransform = selectedObject.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                // Копируем позицию и размер в соответствующие свойства ButtonData
                buttonDataProperty.FindPropertyRelative("position").vector2Value = rectTransform.anchoredPosition;
                buttonDataProperty.FindPropertyRelative("size").vector2Value = rectTransform.sizeDelta;

                // Применяем изменения
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
