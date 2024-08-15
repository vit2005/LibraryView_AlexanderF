using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Outline))]
public class OutlineAnimation : MonoBehaviour
{

    [SerializeField] private Outline outline;
    public float animationSpeed = 1f;
    public float maxDistance = 5f;

    void Update()
    {
        float pingPongValue = Mathf.PingPong(Time.time * animationSpeed, maxDistance);
        outline.effectDistance = new Vector2(pingPongValue, pingPongValue);
    }
}
