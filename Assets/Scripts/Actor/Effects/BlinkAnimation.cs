using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class BlinkAnimation : MonoBehaviour
{
    [SerializeField] Animator animator;

    public float minTime = 2f;
    public float maxTime = 5f;

    private float timeToNextBlink;

    void Start()
    {
        SetRandomTimeToNextBlink();
    }

    void Update()
    {
        timeToNextBlink -= Time.deltaTime;

        if (timeToNextBlink <= 0f)
        {
            animator.SetTrigger("Blink");
            SetRandomTimeToNextBlink();
        }
    }

    void SetRandomTimeToNextBlink()
    {
        timeToNextBlink = Random.Range(minTime, maxTime);
    }
}
