using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Archero
{
    public class UpdateManager : MonoBehaviour, IUpdateManager, IComponent
    {
        private static IUpdateManager instance;

        public static IUpdateManager Instance
        {
            get => instance;
        }

        public event Action onUpdate = delegate {  };
        public event Action onLateUpdate = delegate {  };

        public void Init()
        {
            instance = this;
            
            Reset();
        }

        public void Reset()
        {
            onUpdate = delegate {  };
            onLateUpdate = delegate {  };
        }
        
        private void Update()
        {
            onUpdate();
        }

        private void LateUpdate()
        {
            onLateUpdate();
        }
    }
}