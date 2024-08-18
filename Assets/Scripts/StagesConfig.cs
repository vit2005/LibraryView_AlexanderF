using System;
using System.Collections;
using System.Collections.Generic;
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

[Serializable]
public class ButtonData 
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
}

[Serializable]
public class StageConfig
{
    public Sprite main;
    public AudioClip stageSound;
    public List<ButtonData> buttons;
}

[CreateAssetMenu(fileName = "StagesConfig", menuName = "ScriptableObjects/StagesConfig")]
public class StagesConfig : ScriptableObject
{
    public List<StageConfig> allStages;
}
