using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Archero
{
    public class BattleSceneManager : MonoBehaviour, IComponent, IBattleSceneManager
    {
        [SerializeField] private EnemiesManager enemiesManager;
        
        [SerializeField] private PlayerGeneralController playerGeneralController;
        
        private List<IEnemyController> enemies;

        public List<IEnemyController> Enemies => enemies;
        
        public void Init()
        {
            enemies = new List<IEnemyController>();
            
            ControllersManager.Instance.AddController(this);
            
            enemiesManager.Init();
            
            playerGeneralController.Init();
        }

        public void Reset()
        {
            
        }

        private void OnDestroy()
        {
            ControllersManager.Instance.RemoveController(this);
        }

        public void RegisterEnemy(IEnemyController enemy)
        {
            if(!enemies.Contains(enemy))
                enemies.Add(enemy);
        }

        public void UnregisterEnemy(IEnemyController enemy)
        {
            if(enemies.Contains(enemy))
                enemies.Remove(enemy);
        }
    }
}