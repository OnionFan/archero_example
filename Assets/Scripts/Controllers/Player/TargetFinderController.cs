using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Archero.Data;

namespace Archero
{
    public class TargetFinderController : MonoBehaviour, IComponent, ITargetFinder
    {
        private IBattleSceneManager battleSceneManager;

        private TargetInfo currentTarget;

        private Transform thisTransform;

        public TargetInfo CurrentEnemy => currentTarget;
        
        public void Init()
        {
            UpdateManager.Instance.onUpdate += CustomUpdate;

            battleSceneManager = ControllersManager.Instance.GetController<IBattleSceneManager>();
            
            currentTarget = new TargetInfo();

            thisTransform = transform;
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
            List<IEnemyController> enemies = battleSceneManager.Enemies;

            if (enemies.Count > 0)
            {
                for (int i = 0; i < enemies.Count; i++)
                {
                    float distance = (enemies[i].ThisTransform.position - thisTransform.position).sqrMagnitude;

                    if (currentTarget.controller == null)
                    {
                        currentTarget.controller = enemies[i];
                        currentTarget.distance = distance;
                    }
                    else
                    {
                        if (currentTarget.distance - distance < 0)
                        {
                            currentTarget.controller = enemies[i];
                            currentTarget.distance = distance;
                        }
                    }
                }
            }
            else
            {
                if (currentTarget.controller != null)
                {
                    currentTarget.controller = null;
                }
            }
        }
    }
}