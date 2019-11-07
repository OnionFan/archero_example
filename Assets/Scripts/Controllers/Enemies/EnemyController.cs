using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MOBRitual.Utils;
using UnityEngine;

namespace Archero
{
    public class EnemyController : MonoBehaviour, IEnemyController, IComponent, IDamageReceiver
    {
        [SerializeField] private Component[] components;
        
        [SerializeField] private int maxHp = 100;

        [SerializeField] private GameObject deathEffect;
        
        private Transform thisTransform;

        public Transform ThisTransform => thisTransform;

        private IBattleSceneManager battleSceneManager;

        private int currentHp;
        
        public void Init()
        {
            currentHp = maxHp;
            
            thisTransform = transform;

            battleSceneManager = ControllersManager.Instance.GetController<IBattleSceneManager>();
            
            battleSceneManager.RegisterEnemy(this);

            IComponent[] thisComponents = components.OfType<IComponent>().ToArray();

            for (int i = 0; i < thisComponents.Length; i++)
            {
                thisComponents[i].Init();
            }
        }

        public void Reset()
        {
            
        }

        private void OnDestroy()
        {
            battleSceneManager.UnregisterEnemy(this);
        }

        public void AddDamage(int damage)
        {
            if (damage > 0)
            {
                currentHp -= damage;

                if (currentHp < 0)
                {
                    battleSceneManager.UnregisterEnemy(this);

                    ObjectPool.Spawn(deathEffect, thisTransform.position);
                    
                    GameObject.Destroy(gameObject);
                }
            }
        }
    }
}