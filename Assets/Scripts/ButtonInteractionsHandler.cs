using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonInteractionsHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    private bool _isClickable = false;
    private bool _isLongPress = false;
    private bool _isDraggable = false;
    private string _data;

    public Action<string> OnClickAction;
    public Action<string> OnLongPressBeginAction;
    public Action<string> OnLongPressEndAction;
    public Action<string> OnBeginDragAction;
    public Action<string> OnEndDragAction;

    public float holdTime = 0.5f; // Time in seconds to recognize as a long press

    private bool isPointerDown = false;
    private bool isPointerLongPressDown = false;
    private float pointerDownTimer = 0f;

    public void Init(string data, bool isClickable, bool isLongPress, bool isDragable)
    {
        _data = data;
        _isClickable = isClickable;
        _isLongPress = isLongPress;
        _isDraggable = isDragable;
    }
    #region Click and LongPress

    public void Update()
    {
        // If the pointer is down, we start counting the time
        if (isPointerDown)
        {
            pointerDownTimer += Time.deltaTime;

            // If the pointer has been held down longer than the holdTime, invoke long press
            if (_isLongPress && !isPointerLongPressDown && pointerDownTimer >= holdTime)
            {
                OnLongPressBeginAction?.Invoke(_data);
                isPointerLongPressDown = true;
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPointerDown = true;
        pointerDownTimer = 0f;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isPointerDown)
        {
            return;
        }

        // If pointer up happens before holdTime is reached, it's a click
        if (isPointerLongPressDown)
        {
            OnLongPressEndAction?.Invoke(_data);
            isPointerLongPressDown = false;
        }
        else
        {
            if (_isClickable) OnClickAction?.Invoke(_data);
        }

        isPointerDown = false;
        pointerDownTimer = 0f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPointerDown = false;
        isPointerLongPressDown = false;
        pointerDownTimer = 0f;
        OnLongPressEndAction?.Invoke(_data);
    }
    #endregion

    #region Drag
    public void OnDrag(PointerEventData eventData)
    {
        if (!_isDraggable) return;
        transform.position = eventData.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!_isDraggable) return;
        transform.SetAsLastSibling();
        OnBeginDragAction?.Invoke(_data);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!_isDraggable) return;
        OnEndDragAction?.Invoke(_data);
    }
    #endregion

}
