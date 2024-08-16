using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonEffectsController : MonoBehaviour
{
    [SerializeField] Image targetImage;
    [SerializeField] TextMeshProUGUI text;

    [SerializeField] OutlineAnimation outline;
    [SerializeField] BlinkAnimation blink;
    [SerializeField] OpacityAnimation opacity;
    [SerializeField] PumpAnimation pump;

    private Dictionary<ButtonEffects, Behaviour> effectsDictionary = new Dictionary<ButtonEffects, Behaviour>();

    public void Awake()
    {
        effectsDictionary.Add(ButtonEffects.Outline, outline);
        effectsDictionary.Add(ButtonEffects.Blink, blink);
        effectsDictionary.Add(ButtonEffects.Opacity, opacity);
        effectsDictionary.Add(ButtonEffects.Pump, pump);
    }

    public void SetImage(Sprite sprite)
    {
        targetImage.sprite = sprite;
    }

    public void SetText(string title)
    {
        text.text = title;
    }

    public void SetEffects(ButtonEffects effects)
    {
        foreach (var effect in Enum.GetValues(typeof(ButtonEffects))) 
        { 
            if ((effects & (ButtonEffects)effect) != 0)
                effectsDictionary[(ButtonEffects)effect].enabled = true;
        }
    }
}
