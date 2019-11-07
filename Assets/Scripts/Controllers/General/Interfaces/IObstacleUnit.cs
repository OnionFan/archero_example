using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Archero
{
    public interface IObstacleUnit
    {
        void OnPlayerEnter(IDamageReceiver damageReceiver);
    }
}