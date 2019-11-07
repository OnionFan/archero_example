using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Archero
{
    public interface IBattleSceneManager
    {
        List<IEnemyController> Enemies { get; }
        
        void RegisterEnemy(IEnemyController enemy);
        void UnregisterEnemy(IEnemyController enemy);
    }
}