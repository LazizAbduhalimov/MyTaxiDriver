using PoolSystem.Alternative;
using UnityEngine;

namespace Client
{
    public class AllVehicles : MonoBehaviour
    {
        public CarsConfig CarsConfig;
        public CarsPoolContainer[] CarsPool;
        public static AllVehicles Instance => _instance;
        private static AllVehicles _instance;

        private void Awake()
        {
            CarsConfig.Configurate();
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
            CarsPool = GetComponentsInChildren<CarsPoolContainer>();
        }
    }
}