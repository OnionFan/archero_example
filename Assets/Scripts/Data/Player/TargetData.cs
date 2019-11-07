using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Archero.Data
{
    public class TargetInfo
    {
        public IEnemyController controller;
            
        public float distance;

        public TargetInfo()
        {
        }
            
        public TargetInfo(IEnemyController controller, float distance)
        {
            this.controller = controller;
            this.distance = distance;
        }
    }
}