using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class OpacityAnimation : MonoBehaviour
{
    [SerializeField] private List<Image> images;
    public float animationSpeed = 1f;
    //public float minOpacity = 5f;

    void Update()
    {
        float pingPongValue = Mathf.PingPong(Time.time * animationSpeed, 1f);
        var color = Color.white;
        color.a = pingPongValue;
        foreach (var image in images)
        {
            image.color = color;
        }
    }
}
