using Client.Game;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    public class BonusDoubledCoinsSystems : IEcsRunSystem
    {
        private EcsCustomInject<AllPools> _allPools;
        
        private EcsFilterInject<Inc<EEarnMoney>> _eEarnMoneyFilter = "events";
        private EcsFilterInject<Inc<EDoubledCoinsBonus>> _eDoubledCoinsBonusFilter = "events";
        private EcsPoolInject<EDisplayFloatingCoin> _eDisplayCoin = "events";
        
        private EcsFilterInject<Inc<CDoubledCoinsBonus>> _cDoubledCoinsBonusFilter;
        private EcsPoolInject<CDoubledCoinsBonus> _cDoubledCoinsBonus;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var eventEntity in _eDoubledCoinsBonusFilter.Value)
            {
                _cDoubledCoinsBonus.NewEntity(out _).Invoke(10f);
                _eDoubledCoinsBonusFilter.Pools.Inc1.Del(eventEntity);
            }
            
            foreach (var entity in _cDoubledCoinsBonusFilter.Value)
            {
                ref var doubledCoinsBonus = ref _cDoubledCoinsBonusFilter.Pools.Inc1.Get(entity);
                foreach (var eventEntity in _eEarnMoneyFilter.Value)
                {
                    ref var earnData = ref _eEarnMoneyFilter.Pools.Inc1.Get(eventEntity);
                    var collector = earnData.Collector;
                    var value = collector.TaxiMb.MoneyForCircle; 
                    Earn(value);
                    _eDisplayCoin.NewEntity(out _).Invoke(
                        collector.transform.position.AddY(2f).AddZ(2f), value);
                }
                
                doubledCoinsBonus.PassedTime += Time.deltaTime;
                if (doubledCoinsBonus.Duration <= doubledCoinsBonus.PassedTime)
                    _cDoubledCoinsBonus.Value.Del(entity);
            }
        }

        private void Earn(long value)
        {
            Bank.AddCoins(this, value);
        }
    }
}