using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Random = UnityEngine.Random;

namespace Client
{
    public class HandleBonusSystem : IEcsRunSystem
    {
        private EcsFilterInject<Inc<EGiveRandomBonus>> _eGiveRandomBonusFilter = "events";
        private EcsPoolInject<EBonusCoins> _eBonusCar = "events";
        private EcsPoolInject<EBonusCoins> _eBonusCoins = "events";
        private EcsPoolInject<EBonusCoins> _eBoostCarsBonus = "events";
        private EcsPoolInject<EBonusCoins> _eDoubledCoinsBonus = "events";
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _eGiveRandomBonusFilter.Value)
            {
                var r = Random.Range(1, 5);
                var pool = r switch
                {
                    1 => _eBonusCar,
                    2 => _eBonusCoins,
                    3 => _eDoubledCoinsBonus,
                    4 => _eBoostCarsBonus,
                    _ => _eBonusCoins
                };
                pool.Value.NewEntity(out _);
            }
        }
    }
}