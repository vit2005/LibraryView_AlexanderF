using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

// Объявляем класс редактора для объекта типа ImagesConfig
[CustomEditor(typeof(ImagesConfig))]
public class ImagesConfigEditor : Editor
{
    // Сериализованное свойство для списка allImages
    SerializedProperty allImagesProperty;

    // Метод, который вызывается при активации редактора (например, при выделении объекта в инспекторе)
    private void OnEnable()
    {
        // Находим и сохраняем ссылку на свойство "allImages"
        allImagesProperty = serializedObject.FindProperty("allImages");
    }

    // Основной метод, который определяет, как будет отображаться GUI в инспекторе
    public override void OnInspectorGUI()
    {
        // Обновляем сериализованный объект перед началом редактирования
        serializedObject.Update();

        // Проходимся по каждому элементу списка allImages
        for (int i = 0; i < allImagesProperty.arraySize; i++)
        {
            // Получаем текущий элемент списка allImages
            SerializedProperty imageConfigProperty = allImagesProperty.GetArrayElementAtIndex(i);

            // Начинаем вертикальную группу для визуального группирования элементов в рамку
            EditorGUILayout.BeginVertical("box");

            // Отображаем поле для свойства "main" (основное изображение)
            EditorGUILayout.PropertyField(imageConfigProperty.FindPropertyRelative("main"));

            // Вызываем метод для отображения списка ButtonData
            DrawButtonDataList(imageConfigProperty.FindPropertyRelative("buttons"));

            // Добавляем кнопку для удаления текущего ImageConfig из списка
            if (GUILayout.Button("Remove this ImageConfig"))
            {
                // Удаляем элемент списка по индексу
                allImagesProperty.DeleteArrayElementAtIndex(i);
            }

            // Завершаем вертикальную группу
            EditorGUILayout.EndVertical();

            // Добавляем немного пространства между группами
            EditorGUILayout.Space();
        }

        // Добавляем кнопку для создания нового ImageConfig
        if (GUILayout.Button("Add new ImageConfig"))
        {
            // Вызываем метод для добавления нового элемента в список allImages
            AddNewImageConfig();
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
            EditorGUILayout.PropertyField(buttonDataProperty.FindPropertyRelative("SomeUsefulData"));

            // Начинаем горизонтальную группу для размещения кнопок на одной линии
            EditorGUILayout.BeginHorizontal();

            // Кнопка для копирования позиции и размера из выделенного объекта
            if (GUILayout.Button("Copy from selected Image"))
            {
                // Вызываем метод для копирования данных из выделенного объекта
                CopyFromSelectedImage(buttonDataProperty);
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

    // Метод для добавления нового ImageConfig в список allImages
    private void AddNewImageConfig()
    {
        // Увеличиваем размер массива allImages
        allImagesProperty.arraySize++;
        // Получаем ссылку на новый элемент массива
        SerializedProperty newImageConfig = allImagesProperty.GetArrayElementAtIndex(allImagesProperty.arraySize - 1);

        // Очищаем поля нового ImageConfig
        newImageConfig.FindPropertyRelative("main").objectReferenceValue = null;
        newImageConfig.FindPropertyRelative("buttons").ClearArray();
    }

    // Метод для копирования позиции и размера из выделенного Image
    private void CopyFromSelectedImage(SerializedProperty buttonDataProperty)
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
