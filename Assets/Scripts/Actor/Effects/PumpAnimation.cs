using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpAnimation : MonoBehaviour
{

    [SerializeField] private RectTransform rectTransform;
    public float rotationSpeed = 100f;

    void Start()
    {

    }

    void Update()
    {
        float pingPongValue = Mathf.PingPong(Time.time, 2f);
        rectTransform.localScale = Vector3.one * pingPongValue;

        float rotationAmount = rotationSpeed * Time.deltaTime;
        rectTransform.Rotate(0, 0, rotationAmount);
    }
}
