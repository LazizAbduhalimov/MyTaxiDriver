using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Game
{
    public class BonusBoostAllCarsSystem : IEcsRunSystem
    {
        private EcsFilterInject<Inc<CTaxi, CActive>> _activeTaxies;
        private EcsPoolInject<CSpeedBooster> _cSpeedBooster;

        private EcsFilterInject<Inc<EBoostAllCarsBonus>> _eBoostAllCarsBonus = "events";
        private EcsPoolInject<EBoostSpeed> _eBoostSpeed = "events";
        
        private const float StartDuration = 10f;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var eventEntity in _eBoostAllCarsBonus.Value)
            {
                Debug.Log("Boosting");
                foreach (var entity in _activeTaxies.Value)
                {
                    ref var boost = ref _cSpeedBooster.Value.Get(entity);
                    var duration = StartDuration; 
                    _eBoostSpeed.NewEntity(out _).Invoke(boost.SpeedBoosterMb, duration);
                }
                _eBoostAllCarsBonus.Pools.Inc1.Del(eventEntity);
            }
        }
    }
}