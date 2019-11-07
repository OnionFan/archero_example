using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Archero
{
    public interface IEnemyController
    {
        Transform ThisTransform { get; }
    }
}