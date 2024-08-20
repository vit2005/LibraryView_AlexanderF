using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

// Объявляем класс редактора для объекта типа StagesConfig
[CustomEditor(typeof(StagesConfig))]
public class StagesConfigEditor : Editor
{
    // Сериализованное свойство для списка allStages
    SerializedProperty allStagesProperty;
    private string datastring = "";

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
            EditorGUILayout.PropertyField(StageConfigProperty.FindPropertyRelative("music"));

            // Вызываем метод для отображения списка ActorData
            DrawActorDataList(StageConfigProperty.FindPropertyRelative("actors"));
            EditorGUILayout.PropertyField(StageConfigProperty.FindPropertyRelative("main_url"));
            EditorGUILayout.PropertyField(StageConfigProperty.FindPropertyRelative("music_url"));

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
        if (GUILayout.Button("Add new Stage"))
        {
            // Вызываем метод для добавления нового элемента в список allStages
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

        // Применяем изменения в сериализованном объекте
        serializedObject.ApplyModifiedProperties();
    }

    // Метод для отображения списка ActorData (например, scalable или unscalable)
    private void DrawActorDataList(SerializedProperty listProperty)
    {
        // Отображаем заголовок списка с жирным шрифтом
        EditorGUILayout.LabelField("Actors", EditorStyles.boldLabel);

        // Проходимся по каждому элементу списка ActorData
        for (int i = 0; i < listProperty.arraySize; i++)
        {
            // Получаем текущий элемент списка ActorData
            SerializedProperty ActorDataProperty = listProperty.GetArrayElementAtIndex(i);

            // Начинаем обработку свойства для возможности отката изменений
            EditorGUI.BeginProperty(EditorGUILayout.GetControlRect(true), GUIContent.none, ActorDataProperty);

            // Отображаем поля для свойств ActorData
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


            // Начинаем горизонтальную группу для размещения кнопок на одной линии
            EditorGUILayout.BeginHorizontal();

            // Кнопка для копирования позиции и размера из выделенного объекта
            if (GUILayout.Button("Copy from selected"))
            {
                // Вызываем метод для копирования данных из выделенного объекта
                CopyFromSelectedStage(ActorDataProperty);
            }

            // Кнопка для удаления текущего ActorData из списка
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

        // Добавляем кнопку для добавления нового элемента ActorData в список
        if (GUILayout.Button("Add new Actor"))
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
        newStageConfig.FindPropertyRelative("music").objectReferenceValue = null;
        newStageConfig.FindPropertyRelative("actors").ClearArray();
    }

    // Метод для копирования позиции и размера из выделенного Stage
    private void CopyFromSelectedStage(SerializedProperty ActorDataProperty)
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
                // Копируем позицию и размер в соответствующие свойства ActorData
                ActorDataProperty.FindPropertyRelative("position").vector2Value = rectTransform.anchoredPosition;
                ActorDataProperty.FindPropertyRelative("size").vector2Value = rectTransform.sizeDelta;

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
