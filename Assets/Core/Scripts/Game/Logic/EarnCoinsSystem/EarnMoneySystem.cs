using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Module.Bank;

namespace Client.Game
{
    public class EarnMoneySystem : IEcsRunSystem
    {
        private EcsCustomInject<AllPools> _allPools;
        private EcsFilterInject<Inc<EMerged>> _eMergedFilter = "events";
        private EcsFilterInject<Inc<EEarnMoney>> _eEarnMoneyFilter = "events";
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _eEarnMoneyFilter.Value)
            {
                ref var earnData = ref _eEarnMoneyFilter.Pools.Inc1.Get(entity);
                var collector = earnData.Collector;
                Earn(collector.TaxiMb.MoneyForCircle);
            }

            foreach (var entity in _eMergedFilter.Value)
            {
                ref var mergedData = ref _eMergedFilter.Pools.Inc1.Get(entity);
                Earn(mergedData.Source.MoneyForCircle);
                Earn(mergedData.Target.MoneyForCircle);
            }
        }

        private void Earn(long value)
        {
            Bank.AddCoins(this, value);
        }
    }
}