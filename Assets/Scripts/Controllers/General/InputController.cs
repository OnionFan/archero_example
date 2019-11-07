using System;
using System.Collections;
using System.Collections.Generic;
using Archero;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

namespace Archero
{
    public class InputController : MonoBehaviour, IInputController, IComponent
    {
        public event Action onFire = delegate {  };

        public Vector2 MoveAxis => moveAxis;

        private Vector2 moveAxis;
        
        public void Init()
        {
            moveAxis = Vector2.zero;

            UpdateManager.Instance.onUpdate += CustomUpdate;
            
            ControllersManager.Instance.AddController(this);
        }

        public void Reset()
        {
            
        }

        private void OnDestroy()
        {
            UpdateManager.Instance.onUpdate -= CustomUpdate;
            
            ControllersManager.Instance.RemoveController(this);
        }

        private void CustomUpdate()
        {
            moveAxis = Vector2.zero;
            
            if (Input.GetKey(KeyCode.W))
            {
                moveAxis.y = 1;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                moveAxis.y = -1;
            }

            if (Input.GetKey(KeyCode.A))
            {
                moveAxis.x = -1;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                moveAxis.x = 1;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                onFire();
            }
        }
    }
}