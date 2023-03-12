using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPoolable
{
    void InitializeDespawnAction(Action<GameObject> DespawnAction);
    void ResetState();
}
