using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Archero
{
    public class EnemyMoveController : MonoBehaviour, IComponent, IObstacleUnit
    {
        private enum EnemyState
        {
            Stand, Move, Wait
        }

        [SerializeField] private float waitTime = 1.0f;

        [SerializeField] private float moveSpeed = 5.0f;
        [SerializeField] private float maxDistance = 2.0f;

        [SerializeField] private float timeStop = 1.0f;

        private EnemyState currentState;

        private float fixedTime;

        private Vector3 direction;
        private Vector3 targetPoint;

        private Transform thisTransform;

        private float startY;

        private CapsuleCollider collider;

        private int layers = 1 << 8;
        
        public void Init()
        {
            UpdateManager.Instance.onUpdate += CustomUpdate;

            currentState = EnemyState.Stand;

            fixedTime = Time.unscaledTime + waitTime;

            thisTransform = transform;

            collider = GetComponent<CapsuleCollider>();
            
            startY = thisTransform.position.y;

            layers = ~layers;
        }

        public void Reset()
        {
            
        }

        private void OnDestroy()
        {
            UpdateManager.Instance.onUpdate -= CustomUpdate;
        }

        private void CustomUpdate()
        {
            if (currentState == EnemyState.Stand)
            {
                if (fixedTime < Time.unscaledTime)
                {
                    direction = Random.onUnitSphere;

                    direction = direction.normalized;

                    direction.y = 0;
                    
                    Ray ray = new Ray(thisTransform.position, direction);

                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit, maxDistance, layers))
                    {
                        targetPoint = thisTransform.position + direction * (hit.distance - collider.radius);
                    }
                    else
                    {
                        targetPoint = thisTransform.position + direction * maxDistance;
                    }

                    targetPoint.y = startY;
                    
                    currentState = EnemyState.Move;
                }
            }
            else if (currentState == EnemyState.Move)
            {
                if ((targetPoint - thisTransform.position).sqrMagnitude < 0.1f)
                {
                    currentState = EnemyState.Stand;
                    
                    fixedTime = Time.unscaledTime + waitTime;
                }
                
                thisTransform.position = thisTransform.position + direction * (moveSpeed * Time.deltaTime);
            }
            else if (currentState == EnemyState.Wait)
            {
                if (fixedTime < Time.unscaledTime)
                {
                    currentState = EnemyState.Move;
                }
            }
        }

        public void OnPlayerEnter(IDamageReceiver damageReceiver)
        {
            currentState = EnemyState.Wait;

            fixedTime = Time.unscaledTime + timeStop;

            damageReceiver.AddDamage(0);
        }
    }
}