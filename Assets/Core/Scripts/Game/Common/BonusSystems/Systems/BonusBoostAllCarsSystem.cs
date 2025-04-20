using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client
{
    public class BonusBoostAllCarsSystem : IEcsRunSystem
    {
        private EcsFilterInject<Inc<CTaxi, CActive>> _activeTaxies;
        private EcsPoolInject<CSpeedBooster> _cSpeedBooster;

        private EcsFilterInject<Inc<EBoostAllCarsBonus>> _eBoostAllCarsBonus = "events";
        private EcsPoolInject<EBoostSpeed> _eBoostSpeed = "events";
        
        public void Run(IEcsSystems systems)
        {
            foreach (var eventEntity in _eBoostAllCarsBonus.Value)
            {
                foreach (var entity in _activeTaxies.Value)
                {
                    ref var boost = ref _cSpeedBooster.Value.Get(entity);
                    _eBoostSpeed.NewEntity(out _).Invoke(boost.SpeedBoosterMb);
                }
                _eBoostAllCarsBonus.Pools.Inc1.Del(eventEntity);
            }
        }
    }
}