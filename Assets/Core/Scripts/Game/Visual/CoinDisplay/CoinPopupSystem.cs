using Client.Game;
using Core.Scripts.Game;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using PrimeTween;
using UnityEngine;

namespace Client
{
    public class CoinPopupSystem : IEcsRunSystem
    {
        private EcsCustomInject<AllPools> _allPools;
        private EcsFilterInject<Inc<EEarnMoney>> _eEarnMoneyFilter = "events";
        private EcsFilterInject<Inc<EMerged>> _eMergeFilter = "events";
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _eEarnMoneyFilter.Value)
            {
                ref var earnData = ref _eEarnMoneyFilter.Pools.Inc1.Get(entity);
                var position = earnData.Collector.transform.position;
                var value = earnData.Collector.TaxiMb.MoneyForCircle;
                PopupCoin(position, value);
            }

            foreach (var entity in _eMergeFilter.Value)
            {
                ref var mergedData = ref _eMergeFilter.Pools.Inc1.Get(entity);
                var source = mergedData.Source;
                var target = mergedData.Target;
                PopupCoin(source.Follower.transform.position, source.MoneyForCircle);
                PopupCoin(target.Follower.transform.position, target.MoneyForCircle);
            }
        }

        private void PopupCoin(Vector3 position, long value)
        {
            var poolObject = _allPools.Value.PopupsPool.GetFromPool<Popup>(position.AddY(10f));
            poolObject.Text.text = "+" + value;
            Tween.LocalPositionY(poolObject.transform, poolObject.transform.position.y + 5f, duration: 0.5f, Ease.OutSine)
                .OnComplete(() => poolObject.gameObject.SetActive(false));
        }
    }
}