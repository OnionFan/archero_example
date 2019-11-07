using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Archero
{
    public class BattleManager : MonoBehaviour
    {
        [SerializeField] private ControllersManager controllersManager;
        
        [SerializeField] private UpdateManager updateManager;
        
        [SerializeField] private InputController inputController;

        [SerializeField] private BattleSceneManager battleSceneManager;

        void Start()
        {
            controllersManager.Init();
            updateManager.Init();
            
            inputController.Init();
            
            battleSceneManager.Init();
        }
    }
}