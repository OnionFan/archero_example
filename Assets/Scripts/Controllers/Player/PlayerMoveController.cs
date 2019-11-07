using System.Collections;
using System.Collections.Generic;
using Archero;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.XR;


namespace Arhero
{
    public class PlayerMoveController : MonoBehaviour, IComponent
    {
        [SerializeField] private float moveSpeed = 5.0f;
        
        private IInputController inputController;

        private CharacterController chController;
        
        public void Init()
        {
            inputController = ControllersManager.Instance.GetController<IInputController>();

            UpdateManager.Instance.onUpdate += CustomUpdate;

            chController = GetComponent<CharacterController>();
        }

        public void Reset()
        {
            
        }

        private void CustomUpdate()
        {
            Vector3 moveDirection = new Vector3(inputController.MoveAxis.x, 0, inputController.MoveAxis.y);

            moveDirection = moveDirection * (moveSpeed * Time.deltaTime);

            chController.Move(moveDirection);
        }
    }
}