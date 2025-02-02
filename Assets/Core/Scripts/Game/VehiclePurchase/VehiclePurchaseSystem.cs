using Client.Game;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using LGrid;
using PrimeTween;
using UI.Buttons;
using UnityEngine;

namespace Client
{
    public class VehiclePurchaseSystem : IEcsRunSystem
    {
        private EcsWorldInject _world;
        private EcsCustomInject<Map> _map;
        private EcsCustomInject<AllPools> _allPools;
        private EcsCustomInject<GameData> _gameData;
        private EcsFilterInject<Inc<EBuyVehicleClicked>> _eBuyVehicleClickedFilter;

        private EcsPoolInject<CBuyVehicle> _cBuyVehicle;
        private EcsPoolInject<CActive> _cActive;
        private Sequence? _sequence;

        private int CarLevel => _gameData.Value.GetBuyingCarLevel();
        private int VehicleCost => _gameData.Value.GetVehicleCost();

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _eBuyVehicleClickedFilter.Value)
            {
                if (!_map.Value.HasFreeCell(out var pair)) continue;
                if (!Bank.SpendCoins(this, VehicleCost)) return;
                var button = _cBuyVehicle.Value.Get(entity).Handler.Button;
                var taxiMb = _allPools.Value.CarsPool[CarLevel].GetFromPool(pair.Key);
                var taxiEntity = taxiMb.PackedEntity.FastUnpack();
                taxiMb.Drive();
                pair.Value.IsOccupied = true;
                _cActive.Value.Add(taxiEntity);
                _gameData.Value.PurchaseNumber++;
                SoundManager.Instance.PlayUISound(AllUiSounds.Purchased, pitchRange: 0.1f);
                _sequence?.Complete();
                _sequence = Sequence.Create(2, CycleMode.Yoyo, Ease.InOutSine)
                    .Chain(Tween.Scale(button.transform, 1.2f, duration: 0.1f));
            }
        }
    }
}