using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class StageEffectsController : MonoBehaviour
{
    [SerializeField] private new Transform light;

    public void Clear()
    {
        light.gameObject.SetActive(false);
    }

    public void CheckOnStageEffects(List<(ActorData, ActorInteractionsHandler)> buttonsInstances)
    {
        foreach (var buttonData in buttonsInstances)
        {
            if ((buttonData.Item1.effects & ButtonEffects.Light) != 0)
            {
                light.position = buttonData.Item2.transform.position;
                light.gameObject.SetActive(true);
                buttonData.Item2.OnDragAction += (Vector2 pos) => { light.position = pos; };
            }
        }
    }
}
