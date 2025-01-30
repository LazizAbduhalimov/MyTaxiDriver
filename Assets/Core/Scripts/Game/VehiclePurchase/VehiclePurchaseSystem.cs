using Client.Game;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using LGrid;
using PrimeTween;
using UI.Buttons;

namespace Client
{
    public class VehiclePurchaseSystem : IEcsRunSystem
    {
        private EcsCustomInject<AllPools> _allPools;
        private EcsCustomInject<GameData> _gameData;
        private EcsFilterInject<Inc<EBuyVehicleClicked>> _eBuyVehicleClickedFilter;

        private EcsPoolInject<CBuyVehicle> _cBuyVehicle;
        private Sequence? _sequence;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _eBuyVehicleClickedFilter.Value)
            {
                if (Map.Instance.HasFreeCell(out var pair))
                {
                    var button = _cBuyVehicle.Value.Get(entity).Handler.Button;
                    if (!Bank.SpendCoins(this, _gameData.Value.GetVehicleCost())) return;
                    var vehicle = _allPools.Value.CarsPool[_gameData.Value.GetBuyingCarLevel()].GetFromPool(pair.Key);
                    pair.Value.TaxiBase = vehicle.GetComponent<TaxiBase>();
                    _gameData.Value.PurchaseNumber++;
                    
                    _sequence?.Complete();
                    _sequence = Sequence.Create(2, CycleMode.Yoyo, Ease.InOutSine)
                        .Chain(Tween.Scale(button.transform, 1.2f, duration: 0.1f));
                }
            }
        }
    }
}