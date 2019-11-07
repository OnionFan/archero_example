using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Archero
{
    public interface IInputController
    {
        event Action onFire;
        
        Vector2 MoveAxis { get; }
    }
}