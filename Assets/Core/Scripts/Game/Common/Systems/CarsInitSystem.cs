using Client.Game;
using Client.Game.Test;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    public class CarsInitSystem : IEcsInitSystem
    {
        private EcsWorldInject _world;
        private EcsPoolInject<CTaxi> _cTaxi;
        private EcsPoolInject<CDragObject> _cDragObject;
        
        public void Init(IEcsSystems systems)
        {
            Links.Instance.CarsConfig.Configurate();
            var taxis = Object.FindObjectsOfType<TaxiMb>(true);
            foreach (var taxi in taxis) taxi.Init();
        }
    }
}