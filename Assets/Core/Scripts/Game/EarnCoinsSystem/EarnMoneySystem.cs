using Core.Scripts.Game;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using PrimeTween;

namespace Client.Game
{
    public class EarnMoneySystem : IEcsRunSystem
    {
        private EcsCustomInject<AllPools> _allPools;
        private EcsFilterInject<Inc<EEarnMoney>> _eEarnMoneyFilter = "events";
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _eEarnMoneyFilter.Value)
            {
                ref var earnData = ref _eEarnMoneyFilter.Pools.Inc1.Get(entity);
                var collector = earnData.Collector;
                Earn(collector.TaxiBase.MoneyForCircle);
                var position = collector.transform.position;
                var poolObject = _allPools.Value.PopupsPool.GetFromPool(position.AddY(10f)) as Popup;
                if (poolObject != null)
                {
                    poolObject.Text.text = $"+{collector.TaxiBase.MoneyForCircle}";
                    Tween.LocalPositionY(poolObject.transform, poolObject.transform.position.y + 5f, duration: 0.5f, Ease.OutSine)
                        .OnComplete(() => poolObject.gameObject.SetActive(false));
                }
            }
        }
        
        public void Earn(long value)
        {
            Bank.AddCoins(this, value);
        }
    }
}