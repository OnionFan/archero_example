using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Archero
{
    public interface IUpdateManager
    {
        event Action onUpdate;
        event Action onLateUpdate;
    }
}