using Client.Game;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client
{
    public class BonusDoubledCoinsSystems : IEcsRunSystem
    {
        private EcsCustomInject<AllPools> _allPools;
        
        private EcsFilterInject<Inc<EEarnMoney>> _eEarnMoneyFilter = "events";
        private EcsFilterInject<Inc<EDoubledCoinsBonus>> _eDoubledCoinsBonusFilter = "event";
        
        public void Run(IEcsSystems systems)
        {
            foreach (var eventEntity in _eDoubledCoinsBonusFilter.Value)
            {
                foreach (var entity in _eEarnMoneyFilter.Value)
                {
                    ref var earnData = ref _eEarnMoneyFilter.Pools.Inc1.Get(entity);
                    var collector = earnData.Collector;
                    Earn(collector.TaxiMb.MoneyForCircle);
                }
                
                _eDoubledCoinsBonusFilter.Pools.Inc1.Del(eventEntity);
            }
        }

        private void Earn(long value)
        {
            Bank.AddCoins(this, value);
        }
    }
}