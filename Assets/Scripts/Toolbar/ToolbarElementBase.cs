using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Flags]
public enum ToolbarElementType
{
    PrevBtn = 1 << 0,
    NextBtn = 1 << 1,
    AutoNextBtn = 1 << 2,
    SoundPicker = 1 << 3
}

public abstract class ToolbarElementBase : MonoBehaviour
{
    public abstract ToolbarElementType ToolbarElementType { get; }
    public abstract void Clear();
}
