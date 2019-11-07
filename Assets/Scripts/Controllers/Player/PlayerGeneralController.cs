using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Archero
{
    public class PlayerGeneralController : MonoBehaviour, IComponent, IDamageReceiver
    {
        [SerializeField] private Component[] initComponents;

        private IComponent[] playerControllers;

        public void Init()
        {
            playerControllers = initComponents.OfType<IComponent>().ToArray();

            for (int i = 0; i < playerControllers.Length; i++)
            {
                playerControllers[i].Init();
            }
        }

        public void Reset()
        {
            
        }

        public void AddDamage(int damage)
        {
            Debug.Log("Player Receive Damage");
        }
    }
}