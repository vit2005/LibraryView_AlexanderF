using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActorEffectsController : MonoBehaviour
{
    [SerializeField] Image targetImage;
    [SerializeField] TextMeshProUGUI text;

    [SerializeField] OutlineAnimation outline;
    [SerializeField] BlinkAnimation blink;
    [SerializeField] OpacityAnimation opacity;
    [SerializeField] PumpAnimation pump;

    private Dictionary<ActorBehavior, Behaviour> effectsDictionary = new Dictionary<ActorBehavior, Behaviour>();

    public void Awake()
    {
        effectsDictionary.Add(ActorBehavior.Outline, outline);
        effectsDictionary.Add(ActorBehavior.Blink, blink);
        effectsDictionary.Add(ActorBehavior.Opacity, opacity);
        effectsDictionary.Add(ActorBehavior.Pump, pump);
    }

    public void SetImage(Sprite sprite)
    {
        targetImage.sprite = sprite;
    }

    public void SetText(string title)
    {
        text.text = title;
    }

    public void SetEffects(ActorBehavior effects)
    {
        foreach (var effect in Enum.GetValues(typeof(ActorBehavior))) 
        { 
            if ((effects & (ActorBehavior)effect) != 0 && effectsDictionary.ContainsKey((ActorBehavior)effect))
                effectsDictionary[(ActorBehavior)effect].enabled = true;
        }
    }
}
