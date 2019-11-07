using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Archero
{
    public class EnemiesManager : MonoBehaviour, IComponent
    {
        [SerializeField] private EnemyController[] startedEnemies;
        
        public void Init()
        {
            for (int i = 0; i < startedEnemies.Length; i++)
            {
                startedEnemies[i].Init();
            }
        }

        public void Reset()
        {
        }
    }
}