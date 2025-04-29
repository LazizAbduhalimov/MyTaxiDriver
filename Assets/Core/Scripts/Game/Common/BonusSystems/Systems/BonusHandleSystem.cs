using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Client
{
    public class BonusHandleSystem : IEcsRunSystem
    {
        private EcsFilterInject<Inc<EGiveRandomBonus>> _eGiveRandomBonusFilter = "events";
        private EcsPoolInject<EBonusCar> _eBonusCar = "events";
        private EcsPoolInject<EBonusCoins> _eBonusCoins = "events";
        private EcsPoolInject<EBoostAllCarsBonus> _eBoostAllCarsBonus = "events";
        private EcsPoolInject<EDoubledCoinsBonus> _eDoubledCoinsBonus = "events";
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _eGiveRandomBonusFilter.Value)
            {
                var r = Random.Range(1, 5);
                if (r == 1)
                    _eBonusCar.NewEntity(out _);
                else if (r == 2)
                    _eBonusCoins.NewEntity(out _);
                else if (r == 3)
                    _eDoubledCoinsBonus.NewEntity(out _);
                else if (r == 4)
                    _eBoostAllCarsBonus.NewEntity(out _);
                else
                    _eBonusCoins.NewEntity(out _);
                
                Debug.Log($"Bonus {r}");
                _eGiveRandomBonusFilter.Pools.Inc1.Del(entity);
            }
        }
    }
}