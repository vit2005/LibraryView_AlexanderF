using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageEffectsController : MonoBehaviour
{
    [SerializeField] LightStageEffect lightEffect;

    private Dictionary<ActorBehavior, IStageEffect> effectsDictionary = new Dictionary<ActorBehavior, IStageEffect>();

    public void Awake()
    {
        effectsDictionary.Add(ActorBehavior.Light, lightEffect);
    }

    public void Clear()
    {
        foreach (var effect in effectsDictionary.Values)
        {
            effect.Clear();
        }
    }

    public void CheckOnStageEffects(List<(ActorData, ActorInteractionsHandler)> buttonsInstances)
    {
        foreach (var buttonData in buttonsInstances)
        {
            foreach (var effect in Enum.GetValues(typeof(ActorBehavior)))
            {
                if ((buttonData.Item1.effects & (ActorBehavior)effect) != 0 && effectsDictionary.ContainsKey((ActorBehavior)effect))
                    effectsDictionary[(ActorBehavior)effect].Init(buttonData);
            }
        }
    }
}
