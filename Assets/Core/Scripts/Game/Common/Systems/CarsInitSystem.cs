using Game.Game;
using Game.Game.Test;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Game
{
    public class CarsInitSystem : IEcsInitSystem
    {
        private EcsWorldInject _world;
        private EcsPoolInject<CTaxi> _cTaxi;
        private EcsPoolInject<CDragObject> _cDragObject;
        
        public void Init(IEcsSystems systems)
        {
            var taxis = Object.FindObjectsOfType<TaxiMb>(true);
            foreach (var taxi in taxis) taxi.Init();
        }
    }
}