using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoclickToolbarButton : SimpleToolbarButton
{
    [SerializeField] ActorInteractionsHandler toggle;
    [SerializeField] float autoclickInterval;
    [Space]
    [SerializeField] Image toggleImage;
    [SerializeField] Image arrowImage;
    [SerializeField] Color selectedColor;

    private Coroutine _clickingCoroutine;

    public new void Awake()
    {
        base.Awake();
        toggle.Init(string.Empty, true, false, false);
        toggle.OnClickAction += onClick;
    }

    private void onClick(string obj)
    {
        if (_clickingCoroutine != null)
        {
            StopCoroutine(_clickingCoroutine);
            _clickingCoroutine = null;
            SetColor(Color.white);
        }
        else
        {
            _clickingCoroutine = StartCoroutine(ClickingProcess());
            SetColor(selectedColor);
        }
    }

    private void SetColor(Color color)
    {
        toggleImage.color = color;
        arrowImage.color = color;
    }

    private IEnumerator ClickingProcess()
    {
        while (true)
        {
            yield return new WaitForSeconds(autoclickInterval);
            _onClick?.Invoke(interactionHandler.data);
        }
    }
}
