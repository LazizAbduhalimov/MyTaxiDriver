using System;
using PoolSystem.Alternative;
using UnityEngine;

namespace Client
{
    public class AllVehicles : MonoBehaviour
    {
        public PoolContainer[] CarsPool;
        public static AllVehicles Instance => _instance;
        private static AllVehicles _instance;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                return;
            }
            Destroy(gameObject);
        }
        
        [ContextMenu("GetChildPools")]
        private void GetChildPools()
        {
            CarsPool = GetComponentsInChildren<PoolContainer>();
        }
    }
}