using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SoundType
{
    Mute = 0,
    Human = 1,
    Robot = 2
}

public class ToolbarSoundPicker : ToolbarElementBase
{
    public override ToolbarElementType ToolbarElementType => ToolbarElementType.SoundPicker;

    public Action<SoundType> OnSoundTypeChanged;
    private Action<SoundType> _subscribtion;

    public void DropdownValueChanged(int change)
    {
        OnSoundTypeChanged?.Invoke((SoundType)change);
    }

    public void SubscribeSoundTypeChange(Action<SoundType> onClick)
    {
        OnSoundTypeChanged += onClick;
        _subscribtion = onClick;
    }

    public override void Clear()
    {
        OnSoundTypeChanged -= _subscribtion;
        _subscribtion = null;
    }
}
