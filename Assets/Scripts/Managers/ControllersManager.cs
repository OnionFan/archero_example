using System.Collections;
using System.Collections.Generic;
using Archero;
using UnityEngine;
using System.Linq;

namespace Archero
{
    public class ControllersManager : MonoBehaviour, IControllersManager, IComponent
    {
        private static IControllersManager instance;

        public static IControllersManager Instance => instance;

        private List<object> controllers;

        public void Init()
        {
            instance = this;
            
            Reset();
        }

        public void Reset()
        {
            controllers = new List<object>();
        }

        public void AddController(object controller)
        {
            if(!controllers.Contains(controller))
                controllers.Add(controller);
        }

        public void RemoveController(object controller)
        {
            if(controllers.Contains(controller))
                controllers.Remove(controller);
        }

        public T GetController<T>()
        {
            T[] components = controllers.OfType<T>().ToArray();

            if (components.Length > 0)
            {
                return components[0];
            }
            else
            {
                return default(T);
            }
        }
    }
}