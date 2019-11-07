using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Archero.Data;

namespace Archero
{
    public interface ITargetFinder
    {
        TargetInfo CurrentEnemy { get; }
    }
}