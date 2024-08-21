using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStageEffect
{
    void Clear();
    void Init((ActorData, ActorInteractionsHandler) data);
}
