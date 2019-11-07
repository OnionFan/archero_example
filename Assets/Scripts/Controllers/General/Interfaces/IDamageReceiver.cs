using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Archero
{
    public interface IDamageReceiver
    {
        void AddDamage(int damage);
    }
}