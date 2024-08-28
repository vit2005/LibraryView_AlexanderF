using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleToolbarButton : ToolbarElementBase
{
    public ActorInteractionsHandler interactionHandler;
    [SerializeField] private ToolbarElementType type;

    public override ToolbarElementType ToolbarElementType => type;

    protected Action<string> _onClick;

    public void Awake()
    {
        interactionHandler.Init(string.Empty, true, false, false);
    }

    public void SubscribeClick(Action<string> onClick)
    {
        interactionHandler.OnClickAction += onClick;
        _onClick = onClick;
    }

    public override void Clear()
    {
        interactionHandler.OnClickAction -= _onClick;
        _onClick = null;
    }
}
