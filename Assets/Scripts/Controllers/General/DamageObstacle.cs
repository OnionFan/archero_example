using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Archero
{
    public class DamageObstacle : MonoBehaviour, IComponent
    {
        private IObstacleUnit thisUnit;

        public void Init()
        {
            thisUnit = GetComponent<IObstacleUnit>();
        }

        public void Reset()
        {
            
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                thisUnit.OnPlayerEnter(other.GetComponent<IDamageReceiver>());
            }
        }
    }
}