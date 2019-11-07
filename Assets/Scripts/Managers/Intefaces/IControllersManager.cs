using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Archero
{
    public interface IControllersManager
    {
        void AddController(object controller);

        void RemoveController(object controller);

        T GetController<T>();
    }
}