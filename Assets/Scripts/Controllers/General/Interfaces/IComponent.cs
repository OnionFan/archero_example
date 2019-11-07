using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Archero
{
    public interface IComponent
    {
        void Init();
        void Reset();
    }
}