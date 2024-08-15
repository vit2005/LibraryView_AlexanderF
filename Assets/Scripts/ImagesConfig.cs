using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ButtonData 
{
    public Sprite image;
    public Vector2 position;
    public Vector2 size;
    public bool isScalable;
    public string SomeUsefulData;
}

[Serializable]
public class ImageConfig
{
    public Sprite main;
    public List<ButtonData> buttons;
}

[CreateAssetMenu(fileName = "ImagesConfig", menuName = "ScriptableObjects/ImagesConfig")]
public class ImagesConfig : ScriptableObject
{
    public List<ImageConfig> allImages;
}
