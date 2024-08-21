using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class LightStageEffect : MonoBehaviour, IStageEffect
{
    [SerializeField] private Transform lightImage;
    [SerializeField] private Transform darkness;

    public void Init((ActorData, ActorInteractionsHandler) buttonData)
    {
        darkness.gameObject.SetActive(true);
        lightImage.position = buttonData.Item2.transform.position;
        buttonData.Item2.OnClickAction += (string _) => { 
            lightImage.gameObject.SetActive(!lightImage.gameObject.activeSelf);
            darkness.gameObject.SetActive(!darkness.gameObject.activeSelf); 
        };
        buttonData.Item2.OnDragAction += (Vector2 pos) => { lightImage.position = pos; };
    }

    public void Clear()
    {
        lightImage.gameObject.SetActive(false);
        darkness.gameObject.SetActive(false);
    }

}
