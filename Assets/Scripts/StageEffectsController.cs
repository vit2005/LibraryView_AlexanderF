using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageEffectsController : MonoBehaviour
{
    [SerializeField] private Transform lightImage;

    public void Clear()
    {
        lightImage.gameObject.SetActive(false);
    }

    public void CheckOnStageEffects(List<(ActorData, ActorInteractionsHandler)> buttonsInstances)
    {
        foreach (var buttonData in buttonsInstances)
        {
            if ((buttonData.Item1.effects & ButtonEffects.Light) != 0)
            {
                lightImage.position = buttonData.Item2.transform.position;
                lightImage.gameObject.SetActive(true);
                buttonData.Item2.OnDragAction += (Vector2 pos) => { lightImage.position = pos; };
            }
        }
    }
}
