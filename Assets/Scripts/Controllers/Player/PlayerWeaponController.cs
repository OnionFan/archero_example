using System.Collections;
using System.Collections.Generic;
using Archero.Data;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

namespace Archero
{
    public class PlayerWeaponController : MonoBehaviour, IComponent
    {
        [SerializeField] private int damage = 50;
        
        private ITargetFinder targetFinder;

        private IInputController inputController;

        private Transform thisTransform;
        
        public void Init()
        {
            targetFinder = GetComponent<ITargetFinder>();

            inputController = ControllersManager.Instance.GetController<IInputController>();

            UpdateManager.Instance.onUpdate += CustomUpdate;
            
            thisTransform = transform;

            inputController.onFire += OnFireHandler;
        }

        public void Reset()
        {
            
        }

        private void OnDestroy()
        {
            UpdateManager.Instance.onUpdate -= CustomUpdate;
        }

        private void OnFireHandler()
        {
            TargetInfo currentTarget = targetFinder.CurrentEnemy;
            
            if (currentTarget.controller != null)
            {
                Vector3 direction = (currentTarget.controller.ThisTransform.position - thisTransform.position)
                    .normalized;

                Ray ray = new Ray(thisTransform.position + Vector3.up * 0.2f, direction);

                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, Mathf.Sqrt(currentTarget.distance)))
                {
                    if (hit.transform == currentTarget.controller.ThisTransform)
                    {
                        ((IDamageReceiver)currentTarget.controller).AddDamage(damage);
                    }
                }
            }
        }

        private void CustomUpdate()
        {
            TargetInfo currentTarget = targetFinder.CurrentEnemy;

            if (currentTarget.controller != null)
            {
                Vector3 direction = (currentTarget.controller.ThisTransform.position - thisTransform.position)
                    .normalized;

                direction.y = 0;

                thisTransform.forward = direction;
            }
        }
    }
}