using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[System.Flags]
public enum ButtonEffects
{
    None = 0,
    Outline = 1 << 0,  // 1
    Blink = 1 << 1,  // 2
    Opacity = 1 << 2,    // 4
    Pump = 1 << 3, // 8
    Light = 1 << 4 // 8
}

#region serialization classes

[Serializable]
public class ActorDataSerializable
{
    public Vector2 position;
    public Vector2 size;
    public bool isScalable;
    public string title;
    public ButtonEffects effects;
    public bool isDragable;

    // serialization
    public string image_url;
    public string clickSound_url;
    public string longPressSound_url;

    public ActorDataSerializable(ActorData data)
    {
        position = data.position;
        size = data.size;
        isScalable = data.isScalable;
        title = data.title;
        effects = data.effects;
        isDragable = data.isDragable;
        image_url = data.image_url;
        clickSound_url = data.clickSound_url;
        longPressSound_url = data.longPressSound_url;
    }

    public ActorData GetActorData()
    {
        ActorData result = new ActorData();
        result.position = position;
        result.size = size;
        result.isScalable = isScalable;
        result.title = title;
        result.effects = effects;
        result.isDragable = isDragable;
        result.image_url = image_url;
        result.clickSound_url = clickSound_url;
        result.longPressSound_url = longPressSound_url;

        return result;
    }
}

[Serializable]
public class StageConfigSerializable
{
    public List<ActorDataSerializable> actors;

    // serialization
    public string main_url;
    public string music_url;

    public StageConfigSerializable(StageConfig data)
    {
        actors = new List<ActorDataSerializable>();
        foreach (var actor in data.actors)
        {
            actors.Add(new ActorDataSerializable(actor));
        }
        main_url = data.main_url;
        music_url = data.music_url;
    }

    public StageConfig GetStageConfigData()
    {
        StageConfig result = new StageConfig();

        result.actors = new List<ActorData>();
        foreach (var actor in actors)
        {
            result.actors.Add(actor.GetActorData());
        }
        result.main_url = main_url;
        result.music_url = music_url;

        return result;
    }
}

[Serializable]
public class StagesConfigSerializable
{
    public List<StageConfigSerializable> allStages;

    public StagesConfigSerializable(StagesConfig data)
    {
        allStages = new List<StageConfigSerializable>();
        foreach(var stage in data.allStages)
        {
            allStages.Add(new StageConfigSerializable(stage));
        }
    }

    public void FillStagesConfig(StagesConfig data)
    {
        data.allStages.Clear();
        foreach (var stage in allStages)
        {
            data.allStages.Add(stage.GetStageConfigData());
        }
    }
}

#endregion

[Serializable]
public class ActorData 
{
    public Sprite image;
    public Vector2 position;
    public Vector2 size;
    public bool isScalable;
    public string title;
    public ButtonEffects effects;
    public bool isDragable;
    public AudioClip clickSound;
    public AudioClip longPressSound;

    // serialization
    public string image_url;
    public string clickSound_url;
    public string longPressSound_url;
}

[Serializable]
public class StageConfig
{
    public Sprite main;
    public AudioClip music;
    public List<ActorData> actors;

    // serialization
    public string main_url;
    public string music_url;
}

[CreateAssetMenu(fileName = "StagesConfig", menuName = "ScriptableObjects/StagesConfig")]
public class StagesConfig : ScriptableObject
{
    public List<StageConfig> allStages;
}
