using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ActorInteractionsHandler))]
public class ActorSoundsHandler : MonoBehaviour
{
    [SerializeField] private ActorInteractionsHandler interactionsHandler;
    [SerializeField] private AudioSource audioSource;

    public void Init(AudioClip click, AudioClip longPress)
    {
        if (click != null) interactionsHandler.OnClickAction += (string _) => { audioSource.PlayOneShot(click); };
        if (longPress != null)
        {
            interactionsHandler.OnLongPressBeginAction += (string _) => {
                audioSource.clip = longPress;
                audioSource.Play(); 
            };

            interactionsHandler.OnLongPressEndAction += (string _) =>
            {
                audioSource.Stop();
            };
        }
    }
}
