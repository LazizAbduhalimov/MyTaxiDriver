using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Module.Bank;
using UI.Buttons;
using UnityEngine;

namespace Client
{
    public class VehiclePurchaseButtonStateHandleSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsCustomInject<GameData> _gameData;
        private EcsFilterInject<Inc<EBankValueChanged>> _eBankValueChanged = "events";
        private EcsFilterInject<Inc<CBuyVehicle>> _cBuyVehicleButtonFilter;
        private readonly Color _fadeRed = new (1, 0, 0, 0.3f);
        
        public void Init(IEcsSystems systems)
        {
            foreach (var buttonEntity in _cBuyVehicleButtonFilter.Value)
            {
                ref var buyButton = ref _cBuyVehicleButtonFilter.Pools.Inc1.Get(buttonEntity);
                
                var cost = _gameData.Value.GetVehicleCost();
                var hasCoins = Bank.HasEnoughCoins(cost);
                buyButton.Handler.Button.interactable = hasCoins;
                buyButton.Text.text = $"Buy {cost}";
                buyButton.Text.color = hasCoins ? Color.white : _fadeRed;
            }
        }
        
        public void Run(IEcsSystems systems)
        {
            foreach (var buttonEntity in _cBuyVehicleButtonFilter.Value)
            {
                ref var buyButton = ref _cBuyVehicleButtonFilter.Pools.Inc1.Get(buttonEntity);
                
                foreach (var entity in _eBankValueChanged.Value)
                {
                    ref var changedData = ref _eBankValueChanged.Pools.Inc1.Get(entity);
                    var cost = _gameData.Value.GetVehicleCost();
                    var hasCoins = Bank.HasEnoughCoins(cost);
                    buyButton.Handler.Button.interactable = hasCoins;
                    buyButton.Text.text = $"Buy {cost}";
                    buyButton.Text.color = hasCoins ? Color.white : _fadeRed;
                }   
            }
        }
    }
}